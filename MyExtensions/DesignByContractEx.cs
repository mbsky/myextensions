using System.IO;

namespace System
{
    public partial class Check
    {
        #region Const Literals

        private const string FILENOTEXISTS_FAILING_MESSAGE = "file {0} not exists";
        private const string Between_FAILING_MESSAGE = "{0} {1} {2} {3} {4} requird";
        private const string IsEmailAddress_FAILING_MESSAGE = "{0} is not a valide email address";

        #endregion

        public static ICheckStrategy IsEmailAddress = new IsEmailAddressStrategy();

        public static ICheckStrategy FileExists<T>(T compareValue)
        {
            return new FileExistsStrategy<T>(compareValue);
        }

        public static ICheckStrategy Between<T>(T minValue, T maxValue, bool allowEqual)
        {
            return new BetweenStrategy<T>(minValue, maxValue, allowEqual);
        }

        private sealed class IsEmailAddressStrategy : ICheckStrategy
        {
            #region ICheckStrategy Members

            public bool Pass(object obj)
            {
                if (obj is string)
                {
                   return (obj as string).IsEmailAddress();
                }

                return false;
            }

            public string GetFailingMessage(string objName)
            {
                return string.Format(IsEmailAddress_FAILING_MESSAGE,objName);
            }

            #endregion
        }

        private sealed class FileExistsStrategy<T> : ICheckStrategy
        {
            private T compareValue;

            public FileExistsStrategy(T compareValue)
            {
                this.compareValue = compareValue;
            }

            #region ICheckStrategy Members

            public bool Pass(object obj)
            {
                if (obj == null || compareValue == null)
                    return false;

                else if (compareValue is string)
                {
                    if (string.IsNullOrEmpty(compareValue as string))
                        return false;
                    else
                    {
                        return File.Exists(compareValue as string);
                    }
                }
                else if (compareValue is FileInfo)
                {
                    return (compareValue as FileInfo).Exists;
                }

                return false;
            }

            public string GetFailingMessage(string objName)
            {
                return string.Format(FILENOTEXISTS_FAILING_MESSAGE, objName, compareValue);
            }

            #endregion
        }

        private sealed class BetweenStrategy<T> : ICheckStrategy
        {
            private T minValue;
            private T maxValue;

            bool IsGreaterThan = false;
            bool IsLessThan = false;
            bool AllowEqual = false;

            public BetweenStrategy(T minValue, T maxValue, bool allowEqual)
            {
                this.minValue = minValue;
                this.maxValue = maxValue;
                this.AllowEqual = allowEqual;
            }

            #region ICheckStrategy Members

            public bool Pass(object obj)
            {

                if (AllowEqual)
                {
                    IsGreaterThan = obj is T && ((IComparable)obj).CompareTo(minValue) > 0;

                    IsLessThan = obj is T && ((IComparable)obj).CompareTo(maxValue) < 0;
                }
                else
                {
                    IsGreaterThan = obj is T && ((IComparable)obj).CompareTo(minValue) >= 0;

                    IsLessThan = obj is T && ((IComparable)obj).CompareTo(maxValue) <= 0;
                }

                return IsGreaterThan && IsLessThan;
            }

            public string GetFailingMessage(string objName)
            {
                string lessEqual = AllowEqual ? "<=" : "<";
                string greatEqual = AllowEqual ? ">=" : ">";

                return string.Format(Between_FAILING_MESSAGE, minValue, lessEqual, objName, greatEqual, maxValue);
            }

            #endregion
        }
    }
}

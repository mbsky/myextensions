# Introduction #

ObjectFactory.


# Details #

ObjectFactory Has a method CreateInstance implemented with System.Reflection.Emit:
  * use ObjectFactory's CreateInstance instead of Activator.CreateInstance To **Saveing Your CPU Usage**
> `public static T CreateInstance<T>(params object[] parameters)`
  * It is recommended replace all `Activator.CreateInstance` To `ObjectFactory.CreateInstance` In your project.
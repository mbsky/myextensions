using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;

namespace System.Web.Mvc
{
    /*
     
        protected void Application_Start()
        {
            IUnityContainer container = new UnityContainer();
            container.RegisterType<IService, Service>();
            CustomUnityControllerFactory factory = new CustomUnityControllerFactory(container);
            ControllerBuilder.Current.SetControllerFactory(factory);
            RegisterRoutes(RouteTable.Routes);
        }
     
     */

    public class CustomUnityControllerFactory : DefaultControllerFactory
    {
        private readonly IUnityContainer _container;

        public CustomUnityControllerFactory(IUnityContainer container)
        {
            _container = container;
        }

        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            return _container.Resolve(controllerType) as IController;
        }

    }
}

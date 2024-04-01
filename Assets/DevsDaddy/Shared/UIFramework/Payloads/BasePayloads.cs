using System;
using DevsDaddy.Shared.EventFramework.Core.Payloads;
using DevsDaddy.Shared.UIFramework.Core;

namespace DevsDaddy.Shared.UIFramework.Payloads
{
    [System.Serializable]
    public class OnViewAdded : IPayload
    {
        public IBaseView View;
    }

    [System.Serializable]
    public class OnViewRemoved : IPayload
    {
        public IBaseView View;
    }

    [System.Serializable]
    public class OnViewShown : IPayload
    {
        public IBaseView View;
    }
    
    [System.Serializable]
    public class OnViewHidden : IPayload
    {
        public IBaseView View;
    }

    [System.Serializable]
    public class OnViewUpdated : IPayload
    {
        public Type View;
        public IViewData Data;
    }

    [System.Serializable]
    public class OnViewNavigated : IPayload
    {
        public Type View;
        public IViewData Data;
        public DisplayOptions Display = new DisplayOptions();
    }

    [System.Serializable]
    public class OnViewBackRequested : IPayload
    {
        public Type View;
        public DisplayOptions Display = new DisplayOptions();
    }

    [System.Serializable]
    public class OnViewHomeRequested : IPayload
    {
        public bool CloseAll = true;
    }
}
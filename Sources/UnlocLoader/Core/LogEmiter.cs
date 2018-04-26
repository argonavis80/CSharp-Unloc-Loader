using System;

namespace UnlocLoader.Core
{
    public class LogEmiter
    {
        public EventHandler<string> OnInfo;
        public EventHandler<string> OnWarn;
        public EventHandler<string> OnTrace;

        protected internal void EmitInfo(string message)
        {
            OnInfo?.Invoke(this, message);
        }

        protected internal void EmitWarn(string message)
        {
            OnWarn?.Invoke(this, message);
        }

        protected internal void EmitTrace(string message)
        {
            OnTrace?.Invoke(this, message);
        }
    }
}
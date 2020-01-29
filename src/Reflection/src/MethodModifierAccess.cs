using System.Reflection;

namespace NerdyMishka.Reflection
{
    internal class MethodModifierAccess : ReflectionModifierAccess
    {
        private MethodBase info;

        public MethodModifierAccess(MethodBase info)
        {
            this.info = info;
        }

        public override bool IsStatic => this.info.IsStatic;

        public override bool IsPublic => this.info.IsPublic;

        public override bool IsPrivate => this.info.IsPrivate;

        public override bool IsInstance => !this.info.IsStatic;

        public override bool IsVirtual => false;

        public override bool IsProtected => this.info.IsFamily;

        public override bool IsInternal => this.info.IsAssembly;
    }
}

using System;
using System.Collections.Generic;

namespace WWA.GrainInterfaces.Models
{
    public abstract class UpdatableEntityModel : EntityModel
    {
        public IList<string> UpdatedProperties { get; private set; } = new List<string>();

        public void PropertyChanged(string propertyName)
        {
            if (this.GetType().GetMember(propertyName).Length == 0)
            {
                throw new Exception($"Property '{propertyName}' does not exist on this type");
            }
            this.UpdatedProperties.Add(propertyName);
        }
    }
}

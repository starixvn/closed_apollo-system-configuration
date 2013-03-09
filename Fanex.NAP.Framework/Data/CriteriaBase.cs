// --------------------------------------------------------------------------
// <copyright file="CriteriaBase.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.6.21">Created</change>
//   <change who="Ricky.Nguyen" date="2012.6.25">Update Get Property DbParamaters and SPParam Attribute</change>
//   <change who="Anders.Le" date="2012.6.25">Change property to method</change>
// </history>

namespace Fanex.NAP.Framework.Data
{
    using System;
    using System.Collections;
    using System.Reflection;

    public abstract class CriteriaBase : ICriteria
    {
        public virtual Hashtable GetSpParams()
        {
            var storeProcNameParams = new Hashtable();
            Type type = this.GetType();

            foreach (PropertyInfo propertyInfo in type.GetProperties())
            {
                Type typeOfPropertyAttribute = typeof(SpParamAttribute);
                SpParamAttribute attr = (SpParamAttribute)Attribute.GetCustomAttribute(propertyInfo, 
                    typeOfPropertyAttribute);

                if (attr == null)
                {
                    storeProcNameParams.Add(propertyInfo.Name.ToLower(), propertyInfo.GetValue(this, null));
                }
                else if (attr != null
                    && !attr.Ignored
                    && attr.SqlParamName != null)
                {
                    storeProcNameParams.Add(attr.SqlParamName.ToLower(), propertyInfo.GetValue(this, null));
                }
            }

            return storeProcNameParams;
        }

        public virtual Hashtable AddSpParams(Hashtable storeProcParamName)
        {
            Type type = this.GetType();

            foreach (PropertyInfo propertyInfo in type.GetProperties())
            {
                Type typeOfPropertyAttribute = typeof(SpParamAttribute);
                SpParamAttribute attr = (SpParamAttribute)Attribute.GetCustomAttribute(propertyInfo,
                    typeOfPropertyAttribute);

                if (attr == null)
                {
                    storeProcParamName.Add(propertyInfo.Name.ToLower(), propertyInfo.GetValue(this, null));
                }
                else if (attr != null
                    && !attr.Ignored
                    && attr.SqlParamName != null)
                {
                    storeProcParamName.Add(attr.SqlParamName.ToLower(), propertyInfo.GetValue(this, null));
                }
            }

            return storeProcParamName;
        }

        public virtual bool IsValid()
        {
            throw new NotImplementedException();
        }

        public virtual string GetSettingKey()
        {
            throw new NotImplementedException();
        }
    }
}

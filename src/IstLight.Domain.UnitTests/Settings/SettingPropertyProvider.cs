// Copyright 2012 Jakub Niemyjski
//
// This file is part of IstLight.
//
// IstLight is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// IstLight is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with IstLight.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Ploeh.AutoFixture.Kernel;

namespace IstLight.UnitTests.Settings
{
    public partial class SettingPropertyProvider : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            foreach (var settingType in SimulationSettingsTests.AllISettingImplementations)
            {
                var orgSetting = SimulationSettingsTests.CreateSetting(settingType);
                var properties = FindAndChangeDefaultProperties(orgSetting);
                object clonedSetting = orgSetting.Clone();
                foreach (var property in properties)
                    yield return new object[]
                        {
                            property.GetValue(orgSetting, null),
                            property.GetValue(clonedSetting, null),
                            property
                        };
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    public partial class SettingPropertyProvider
    {
        public static IEnumerable<PropertyInfo> FindAndChangeDefaultProperties(object obj)
        {
            var fixture = new Fixture().Customize(new MultipleCustomization()).Customize(new AutoMoqCustomization());

            Type objectType = obj.GetType();
            var properties = objectType.GetProperties().Where(p => p.CanRead && p.CanWrite);
            foreach (var p in properties)
            {
                object defaultValue = p.GetValue(obj, null);
                object customValue;

                int tryCount = 0;
                try
                {
                    do customValue = CreateAnonymousFromSystemType(fixture, p.PropertyType);
                    while (object.Equals(customValue, defaultValue) && tryCount++ < 5);

                    if (tryCount == 5 && !p.PropertyType.IsValueType)
                        customValue = null;
                }
                catch (ArgumentException) //argument constraint fired, cannot dynamically create object
                {
                    customValue = defaultValue;
                }

                p.SetValue(obj, customValue, null);
            }

            return properties;
        }

        public static object CreateAnonymousFromSystemType(IFixture fixture, Type type)
        {
            MethodInfo method = typeof(SpecimenFactory).GetMethod("CreateAnonymous", new Type[] { typeof(ISpecimenBuilderComposer) });
            MethodInfo generic = method.MakeGenericMethod(type);
            return generic.Invoke(null, new object[] { fixture });
        }
    }
}

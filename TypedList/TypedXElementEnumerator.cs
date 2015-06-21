using System.Collections.Generic;
using System.Linq;

namespace System.ComponentModel {
	using System.Xml.Linq;
	using System.Collections;

	public static class Extensions {
		static public TypedXElement.Enumerator ToTypedElements(this XElement xe) {
			return new TypedXElement.Enumerator(xe);
		}
	}
	public class TypedXElement : CustomTypeDescriptor {
		public class Enumerator : IEnumerable<TypedXElement>, ITypedList {
			PropertyDescriptorCollection ITypedList.GetItemProperties(PropertyDescriptor[] listAccessors) {
				return TypedXElement.Properties;
			}
			string ITypedList.GetListName(PropertyDescriptor[] listAccessors) { return null; }
			IEnumerator IEnumerable.GetEnumerator() { return this.GetEnumerator(); }
			public IEnumerator<TypedXElement> GetEnumerator() { return rows.Select(r => r).GetEnumerator(); }

			TypedXElement[] rows;
			public Enumerator(XElement xe) {
				var adic = xe.Elements().SelectMany(e => e.Attributes())
					.GroupBy(e => e.Name.LocalName).ToDictionary(a => a.Key, a => a.FirstOrDefault());
				TypedXElement.Properties = new PropertyDescriptorCollection(adic.Keys.Select(n => new Descriptor(n)).ToArray(), true);
				this.rows = xe.Elements().Select(e => new TypedXElement(e)).ToArray();
			}
		}
		public class Descriptor : PropertyDescriptor {
			public Descriptor(string name) : base(name, new Attribute[0]) { }
			public override bool CanResetValue(object component) { return false; }
			public override Type ComponentType { get { return typeof(TypedXElement); } }
			public override object GetValue(object component) {
				var row = (TypedXElement)component;
				return row[Name];
			}
			public override bool IsReadOnly { get { return false; } }
			public override Type PropertyType { get { return typeof(string); } }
			public override void ResetValue(object component) {
				throw new NotSupportedException();
			}
			public override void SetValue(object component, object value) {
				var row = (TypedXElement)component;
				row[Name] = string.Format("{0}", value ?? string.Empty);
			}
			public override bool ShouldSerializeValue(object component) { return false; }
		}

		public static PropertyDescriptorCollection Properties { get; set; }
		public XElement XElement { get; private set; }
		public TypedXElement(XElement xe) { this.XElement = xe; }
		public override PropertyDescriptorCollection GetProperties() { return Properties; }
		public string this[string name] {
			get { return this.XElement.Attribute(name) != null ? this.XElement.Attribute(name).Value : null; }
			set { XElement.SetAttributeValue(name, value); }
		}
	}
}

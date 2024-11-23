using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Plugin.LoadedAssemblies.ResxBuilder
{
	/// <summary>.resource в сборке, в котором сохранены Ключ/Значение ресурсов</summary>
	public class ResourceItem : IEnumerable<KeyValuePair<String, Object>>
	{
		private readonly Dictionary<String, Object> _resources;

		/// <summary>Наименование ресурса в сборке</summary>
		public String ResourceName { get; }

		/// <summary>Описание ресурса в сборке</summary>
		public String Description { get; set; }

		/// <summary>Видимость ресурса для других объектов</summary>
		public ResourceAttributes Attributes { get; set; }

		/// <summary>Создание экземпляра ресурса, с указанием наименования ресурса в сборке</summary>
		/// <param name="resourceName">Наименование ресурса в сборке</param>
		public ResourceItem(String resourceName)
		{
			this.ResourceName = resourceName;
			this._resources = new Dictionary<String, Object>();
			this.Attributes = ResourceAttributes.Public;
		}

		/// <summary>Добавить элемент ресурса</summary>
		/// <param name="name">Ключ элемента ресурса</param>
		/// <param name="value">Значение ресурса</param>
		/// <returns>this</returns>
		public ResourceItem AddResource(String name, Object value)
		{
			this._resources.Add(name, value);
			return this;
		}

		/// <summary>Получить список всех ресурсов</summary>
		/// <returns>Ключ/значение ресурсов</returns>
		public IEnumerator<KeyValuePair<String, Object>> GetEnumerator()
		{
			foreach(var item in this._resources)
				yield return item;
		}

		IEnumerator IEnumerable.GetEnumerator()
			=> this.GetEnumerator();
	}
}
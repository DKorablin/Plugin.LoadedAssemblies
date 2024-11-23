using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Resources;
using System.Threading;

namespace Plugin.LoadedAssemblies.ResxBuilder
{
	/// <summary>Компилятор ресурсов</summary>
	public class ResourceBuilder
	{
		private readonly String _assemblyName;
		private readonly CultureInfo _culture;

		private readonly Dictionary<String, ResourceItem> _resources;

		/// <summary>Уникальное наименование ресурса</summary>
		/// <param name="resourceName">Наименование ресурса</param>
		/// <returns>.resource в сборке</returns>
		public ResourceItem this[String resourceName]
		{
			get => this._resources.TryGetValue(resourceName, out ResourceItem result) ? result : null;
			set
			{
				if(value == null)
					this._resources.Remove(resourceName);
				else
					this._resources[resourceName] = value;
			}
		}

		/// <summary>Создание компилятора ресурсов, указав наименование сборки для которой указываются ресурсы и язык</summary>
		/// <param name="assemblyName">Наименование сборки для которой создаются ресурсы</param>
		/// <param name="culture">Наименование языка для которого создаются ресурсы</param>
		public ResourceBuilder(String assemblyName, CultureInfo culture)
		{
			if(String.IsNullOrEmpty(assemblyName))
				throw new ArgumentNullException(assemblyName);

			this._assemblyName = assemblyName;
			this._culture = culture;
			this._resources = new Dictionary<String, ResourceItem>();
		}

		/// <summary>Добавить .resource в компилятор</summary>
		/// <param name="item">.resource с уникальным наименованием</param>
		public void AddResource(ResourceItem item)
			=> this._resources.Add(item.ResourceName, item);

		private AppDomain CreateDomain()
		{
			AppDomainSetup setup = AppDomain.CurrentDomain.SetupInformation;
			setup.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
			return AppDomain.CreateDomain("Resource Builder", AppDomain.CurrentDomain.Evidence, setup);
		}

		/// <summary>Сохранить созданную сборку с ресурсами по определённому пути</summary>
		/// <param name="assemblyPath">Путь по которому сохранить полученную сборку</param>
		public void Save(String assemblyPath)
		{
			if(String.IsNullOrEmpty(assemblyPath))
				throw new ArgumentNullException(nameof(assemblyPath));

			if(this._resources.Count == 0)
				throw new ArgumentException("Resources are not specified");

			AssemblyName asmName = new AssemblyName()
			{
				Name = Path.GetFileNameWithoutExtension(this._assemblyName) + ".resources",
				CodeBase = assemblyPath,
				CultureInfo = this._culture,
			};

			//TODO: Add new domain
			AssemblyBuilder asmBuilder = Thread.GetDomain().DefineDynamicAssembly(asmName, AssemblyBuilderAccess.RunAndSave, assemblyPath);
			ModuleBuilder moduleBuilder = asmBuilder.DefineDynamicModule(asmName.Name + ".dll", asmName.Name + ".dll");

			foreach(KeyValuePair<String, ResourceItem> resource in this._resources)
			{
				IResourceWriter resWriter = moduleBuilder.DefineResource(resource.Value.ResourceName, resource.Value.Description, resource.Value.Attributes);
				foreach(KeyValuePair<String, Object> item in resource.Value)
					resWriter.AddResource(item.Key, item.Value);
			}

			asmBuilder.Save(asmName.Name + ".dll");
		}
	}
}
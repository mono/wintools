namespace Mfconsulting.General.Prj2Make.Schema.Csproj2005
{
	using System.Xml.Serialization;


	/// <remarks/>
	[System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
	public class Compile {
    
	    /// <remarks/>
	    public SubType SubType;
    
	    /// <remarks/>
	    [System.Xml.Serialization.XmlIgnoreAttribute()]
	    public bool SubTypeSpecified;
    
	    /// <remarks/>
	    public string AutoGen;
    
	    /// <remarks/>
	    public string DependentUpon;
    
	    /// <remarks/>
	    public string DesignTimeSharedInput;
    
	    /// <remarks/>
	    [System.Xml.Serialization.XmlAttributeAttribute()]
	    public string Include;
	}

	/// <remarks/>
	[System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
	public enum SubType {
    
	    /// <remarks/>
	    Designer,
    
	    /// <remarks/>
	    Form,
	}

	/// <remarks/>
	[System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
	public class Configuration {
    
	    /// <remarks/>
	    [System.Xml.Serialization.XmlAttributeAttribute()]
	    public string Condition;
    
	    /// <remarks/>
	    [System.Xml.Serialization.XmlTextAttribute()]
	    public string Value;
	}

	/// <remarks/>
	[System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
	public enum DebugType {
    
	    /// <remarks/>
	    full,
    
	    /// <remarks/>
	    pdbonly,
	}

	/// <remarks/>
	[System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
	public class EmbeddedResource {
    
	    /// <remarks/>
	    public Generator Generator;
    
	    /// <remarks/>
	    [System.Xml.Serialization.XmlIgnoreAttribute()]
	    public bool GeneratorSpecified;
    
	    /// <remarks/>
	    public LastGenOutput LastGenOutput;
    
	    /// <remarks/>
	    [System.Xml.Serialization.XmlIgnoreAttribute()]
	    public bool LastGenOutputSpecified;
    
	    /// <remarks/>
	    public SubType SubType;
    
	    /// <remarks/>
	    public string DependentUpon;
    
	    /// <remarks/>
	    [System.Xml.Serialization.XmlAttributeAttribute()]
	    public string Include;
	}

	/// <remarks/>
	[System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
	public enum Generator {
    
	    /// <remarks/>
	    ResXFileCodeGenerator,
    
	    /// <remarks/>
	    SettingsSingleFileGenerator,
	}

	/// <remarks/>
	[System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
	public enum LastGenOutput {
    
	    /// <remarks/>
	    [System.Xml.Serialization.XmlEnumAttribute("Resources.Designer.cs")]
	    ResourcesDesignercs,
    
	    /// <remarks/>
	    [System.Xml.Serialization.XmlEnumAttribute("Settings.Designer.cs")]
	    SettingsDesignercs,
	}

	/// <remarks/>
	[System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
	public class Import {
    
	    /// <remarks/>
	    [System.Xml.Serialization.XmlAttributeAttribute()]
	    public string Project;
	}

	/// <remarks/>
	[System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
	public class ItemGroup {
    
	    /// <remarks/>
	    [System.Xml.Serialization.XmlElementAttribute("None", typeof(None))]
	    [System.Xml.Serialization.XmlElementAttribute("Compile", typeof(Compile))]
	    [System.Xml.Serialization.XmlElementAttribute("EmbeddedResource", typeof(EmbeddedResource))]
	    [System.Xml.Serialization.XmlElementAttribute("Reference", typeof(Reference))]
	    public object[] Items;
	}

	/// <remarks/>
	[System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
	public class None {
    
	    /// <remarks/>
	    public Generator Generator;
    
	    /// <remarks/>
	    public LastGenOutput LastGenOutput;
    
	    /// <remarks/>
	    [System.Xml.Serialization.XmlAttributeAttribute()]
	    public string Include;
	}

	/// <remarks/>
	[System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
	public class Reference {
    
	    /// <remarks/>
	    [System.Xml.Serialization.XmlAttributeAttribute()]
	    public string Include;
	}

	/// <remarks/>
	[System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
	public enum OutputPath {
    
	    /// <remarks/>
	    [System.Xml.Serialization.XmlEnumAttribute("bin\\Debug\\")]
	    binDebug,
    
	    /// <remarks/>
	    [System.Xml.Serialization.XmlEnumAttribute("bin\\Release\\")]
	    binRelease,
	}

	/// <remarks/>
	[System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
	public class Platform {
    
	    /// <remarks/>
	    [System.Xml.Serialization.XmlAttributeAttribute()]
	    public string Condition;
    
	    /// <remarks/>
	    [System.Xml.Serialization.XmlTextAttribute()]
	    public string Value;
	}

	/// <remarks/>
	[System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
	public class Project {
    
	    /// <remarks/>
	    [System.Xml.Serialization.XmlElementAttribute("PropertyGroup")]
	    public PropertyGroup[] PropertyGroup;
    
	    /// <remarks/>
	    [System.Xml.Serialization.XmlArrayItemAttribute(typeof(None), IsNullable=false)]
	    [System.Xml.Serialization.XmlArrayItemAttribute(typeof(Compile), IsNullable=false)]
	    [System.Xml.Serialization.XmlArrayItemAttribute(typeof(EmbeddedResource), IsNullable=false)]
	    [System.Xml.Serialization.XmlArrayItemAttribute(typeof(Reference), IsNullable=false)]
	    public object[][] ItemGroup;
    
	    /// <remarks/>
	    public Import Import;
    
	    /// <remarks/>
	    [System.Xml.Serialization.XmlAttributeAttribute()]
	    public string DefaultTargets;
	}

	/// <remarks/>
	[System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
	public class PropertyGroup {
    
	    /// <remarks/>
	    public Configuration Configuration;
    
	    /// <remarks/>
	    public Platform Platform;
    
	    /// <remarks/>
	    public string ProductVersion;
    
	    /// <remarks/>
	    public System.Decimal SchemaVersion;
    
	    /// <remarks/>
	    [System.Xml.Serialization.XmlIgnoreAttribute()]
	    public bool SchemaVersionSpecified;
    
	    /// <remarks/>
	    public string ProjectGuid;
    
	    /// <remarks/>
	    public string OutputType;
    
	    /// <remarks/>
	    public string AppDesignerFolder;
    
	    /// <remarks/>
	    public string RootNamespace;
    
	    /// <remarks/>
	    public string AssemblyName;
    
	    /// <remarks/>
	    public bool DebugSymbols;
    
	    /// <remarks/>
	    [System.Xml.Serialization.XmlIgnoreAttribute()]
	    public bool DebugSymbolsSpecified;
    
	    /// <remarks/>
	    public DebugType DebugType;
    
	    /// <remarks/>
	    [System.Xml.Serialization.XmlIgnoreAttribute()]
	    public bool DebugTypeSpecified;
    
	    /// <remarks/>
	    public bool Optimize;
    
	    /// <remarks/>
	    [System.Xml.Serialization.XmlIgnoreAttribute()]
	    public bool OptimizeSpecified;
    
	    /// <remarks/>
	    public bool AllowUnsafeBlocks;
    
	    /// <remarks/>
	    [System.Xml.Serialization.XmlIgnoreAttribute()]
	    public bool AllowUnsafeBlocksSpecified;
    
	    /// <remarks/>
	    public OutputPath OutputPath;
    
	    /// <remarks/>
	    [System.Xml.Serialization.XmlIgnoreAttribute()]
	    public bool OutputPathSpecified;
    
	    /// <remarks/>
	    public string DefineConstants;
    
	    /// <remarks/>
	    public string ErrorReport;
    
	    /// <remarks/>
	    public System.SByte WarningLevel;
    
	    /// <remarks/>
	    [System.Xml.Serialization.XmlIgnoreAttribute()]
	    public bool WarningLevelSpecified;
    
	    /// <remarks/>
	    [System.Xml.Serialization.XmlAttributeAttribute()]
	    public string Condition;
	}
}

--ImEditor Project 

project "ImEditor"
	kind "WindowedApp"
	language "C#"

	targetdir ("%{wks.location}/bin/" .. outputdir .. "/%{prj.name}")
	objdir ("%{wks.location}/bin-obj/" .. outputdir .. "/%{prj.name}")

	flags { 
		"WPF"
	}
	files {
		"src/**.cs",
		"src/**.xaml"
	}
	links {
		"Microsoft.Csharp",
		"PresentationCore",
		"PresentationFramework",
		"System",
		"System.Core",
		"System.Data",
		"System.Data.DataSetExtensions",
		"System.Xaml",
		"System.Xml",
		"System.Xml.Linq",
		"System.Runtime.Serialization",
		"System.Numerics",
		"WindowsBase",
	}
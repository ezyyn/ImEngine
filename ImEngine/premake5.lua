
--ImEngine Project

project "ImEngine"
	kind "StaticLib"
	language "C++"
	cppdialect "C++17"

	targetdir ("%{wks.location}/bin/" .. outputdir .. "/%{prj.name}")
	objdir ("%{wks.location}/bin-obj/" .. outputdir .. "/%{prj.name}")

	files {
		"src/**.cs",
		"src/**.cpp"
	}

	defines {
		"_CRT_SECURE_NO_WARNINGS"
	}

	includedirs {
		"src",
	}
	links {
	}

	filter "system:windows"
		systemversion "latest"

		defines {
			"HU_PLATFORM_WINDOWS"
		}

		filter "configurations:Debug"
			defines "IM_DEBUG"
			runtime "Debug"
			symbols "on"

		filter "configurations:Release"
			defines "IM_RELEASE"
			runtime "Release"
			optimize "on"
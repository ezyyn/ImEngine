
--ImEngine Workspace Solution

include "./vendor/premake/premake_customization/solution_items.lua"

workspace "ImEngine"
	architecture "x86_64"
	startproject "ImEditor"

	configurations {
		"Debug",
		"Release"
	}

	flags {
		"MultiProcessorCompile"
	}

outputdir = "%{cfg.buildcfg}-%{cfg.system}-%{cfg.architecture}";

group "Dependencies"
group ""

include "ImEngine"
include "ImEditor"
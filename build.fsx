// include Fake lib
#r @"tools\FAKE\tools\FakeLib.dll"

open System.IO
open Fake

// Properties
let artifactsDir = @"./artifacts/"
let artifactsBuildDir = @"./artifacts/build/"
let artifactsNuGetDir = @"./artifacts/nuget/"
let androidProject = @"./Source.Android/Moq.Android.csproj"
let androidNuspec = @"./Moq.Android.nuspec"
let projects =  [androidProject]

// Targets

Target "Clean" (fun _ ->
    trace "Cleanup..."
    
    CleanDirs [artifactsDir]
)

Target "Build" (fun _ ->
   trace "Building..."
   
   MSBuildRelease artifactsBuildDir "Build" projects
   |> Log "AppBuild-Output: "
)

Target "PackDroid" (fun _ ->
    trace "Packaging for Xamarin.Android ..."

    CreateDir artifactsNuGetDir
    
    NuGet (fun p -> 
        {p with
            Project = "Moq"                              
            OutputPath = artifactsNuGetDir
            WorkingDir = artifactsBuildDir
            Dependencies = ["Castle.Core", "3.3.3-droid"]
            Version = "4.1-droid"}) 
            androidNuspec
)

Target "Publish" (fun _ -> 
    let path = Path.Combine(artifactsNuGetDir, "*.nupkg")
    PublishArtifact path
)

// Dependencies
"Clean"
  ==> "Build"
  ==> "PackDroid"
  ==> "Publish"

// start build
RunTargetOrDefault "Publish"
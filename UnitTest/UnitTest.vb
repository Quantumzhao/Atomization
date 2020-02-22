Imports NUnit.Framework
Imports LCGuidebook
Imports LCGuidebook.Core
Imports LCGuidebook.Core.DataStructures


Namespace LCGuidebook.UnitTest

    Public Class Tests

        <SetUp>
        Public Sub Setup()
            GameManager.InitializeAll()
        End Sub

        <Test>
        Public Sub TestBasicOperation()
            Dim original = ResourceManager.Me.NuclearPlatforms.Count
            Superpower.EnrollNukeStrikePlatfrom(Platform.Types.Silo)
            Dim res = ResourceManager.Me.NuclearPlatforms.Count
            Assert.IsTrue(original = res)
        End Sub

    End Class

End Namespace
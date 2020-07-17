Imports NUnit.Framework
Imports LCGuidebook
Imports LCGuidebook.Core
Imports LCGuidebook.Core.DataStructures
Imports LCGuidebook.Initializer.Manager
Imports System.IO

Namespace LCGuidebook.UnitTest

    Public Class Tests

        <SetUp>
        Public Sub Setup()
            ResourceManager.Misc.SolutionPath = $"{Directory.GetCurrentDirectory()}/../../../.."
            InitializationManager.InitializeAll()
        End Sub

        'don't use it for now
        Public Sub TestBasicOperation()

            Dim original = ResourceManager.Me.NuclearPlatforms.Count
            REM CType(ResourceManager.Me.MainCommandGroups(0).Actions(0), Execution).AssignArgument(Platform.Types.Silo, 0)
            CType(ResourceManager.Me.MainCommandGroups(0).Actions(0), Execution).Execute()
            Dim res = ResourceManager.Me.NuclearPlatforms.Count
            Assert.IsTrue(original = res)

            Dim flag = False
            AddHandler EventManager.TaskProgressAdvenced,
                Sub(sender, e)
                    If e.IsTaskFinished Then
                        flag = True
                    End If
                End Sub
            For index = 1 To 4
                GameManager.TimeElapse()
            Next
            If flag Then
                Assert.Pass()
            Else
                Assert.Fail()
            End If

            If ResourceManager.Me.GetFigureByName("Economy").Growth.Items.Count <> 0 Then
                Assert.Pass()
            End If

        End Sub

        <Test>
        Public Sub TestXmlLoadingOfDemo()

            Assert.AreEqual(ResourceManager.NumOfFigures, 0)

            Assert.AreEqual(ResourceManager.Regions.Count, 9)
        End Sub

    End Class

End Namespace
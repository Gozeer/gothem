Imports System.Web.Mvc
Imports System.Web.WebPages

Namespace gozeer.web.mvc


    Public Class ViewEngine
        Inherits VirtualPathProviderViewEngine
        ''' <summary>
        ''' 0 Theme
        ''' 1 Controller
        ''' 2 Action
        ''' 3 Plugin
        ''' 4 Area
        ''' </summary>
        Private Const CacheKeyFormat As String = ":ViewCacheEntry:{0}:{1}:{2}:{3}:{4}"

        Public PluginViewLocationFormats As String()
        Public AreaPluginViewLocationFormats As String()


        Public Sub New()
            MyBase.FileExtensions = New String() {"vbhtml", "cshtml", "aspx"}


            MyBase.ViewLocationFormats = New String() {
                "~/lookandfeel/{0}/Views/{2}/{1}.aspx",
                "~/lookandfeel/{0}/Views/{2}/{1}.ascx",
                "~/lookandfeel/{0}/Views/Shared/{1}.aspx",
                "~/lookandfeel/{0}/Views/Shared/{1}.ascx",
                "~/lookandfeel/{0}/Views/{2}/{1}.cshtml",
                "~/lookandfeel/{0}/Views/{2}/{1}.vbhtml",
                "~/lookandfeel/{0}/Views/Shared/{1}.cshtml",
                "~/lookandfeel/{0}/Views/Shared/{1}.vbhtml"
            }
            MyBase.PartialViewLocationFormats = MyBase.ViewLocationFormats
            MyBase.MasterLocationFormats = MyBase.ViewLocationFormats

            MyBase.AreaViewLocationFormats = New String() {
               "~/Areas/{4}/{0}/Views/{2}/{1}.aspx",
               "~/Areas/{4}/{0}/Views/{2}/{1}.ascx",
               "~/Areas/{4}/{0}/Views/Shared/{1}.aspx",
               "~/Areas/{4}/{0}/Views/Shared/{1}.ascx",
               "~/Areas/{4}/{0}/Views/{2}/{1}.cshtml",
               "~/Areas/{4}/{0}/Views/{2}/{1}.vbhtml",
               "~/Areas/{4}/{0}/Views/Shared/{1}.cshtml",
               "~/Areas/{4}/{0}/Views/Shared/{1}.vbhtml"}
            MyBase.AreaPartialViewLocationFormats = MyBase.AreaViewLocationFormats
            MyBase.AreaMasterLocationFormats = MyBase.AreaViewLocationFormats

            Me.PluginViewLocationFormats = New String() {
                "~/lookandfeel/{0}/plugins/{3}/Views/{2}/{1}.aspx",
                "~/lookandfeel/{0}/plugins/{3}/Views/{2}/{1}.ascx",
                "~/lookandfeel/{0}/plugins/{3}/Views/Shared/{1}.aspx",
                "~/lookandfeel/{0}/plugins/{3}/Views/Shared/{1}.ascx",
                "~/lookandfeel/{0}/plugins/{3}/Views/{2}/{1}.cshtml",
                "~/lookandfeel/{0}/plugins/{3}/Views/{2}/{1}.vbhtml",
                "~/lookandfeel/{0}/plugins/{3}/Views/Shared/{1}.cshtml",
                "~/lookandfeel/{0}/plugins/{3}/Views/Shared/{1}.vbhtml"
            }

            Me.AreaPluginViewLocationFormats = New String() {
               "~/Areas/{4}/{0}/plugins/{3}/Views/{2}/{1}.aspx",
               "~/Areas/{4}/{0}/plugins/{3}/Views/{2}/{1}.ascx",
               "~/Areas/{4}/{0}/plugins/{3}/Views/Shared/{1}.aspx",
               "~/Areas/{4}/{0}/plugins/{3}/Views/Shared/{1}.ascx",
               "~/Areas/{4}/{0}/plugins/{3}/Views/{2}/{1}.cshtml",
               "~/Areas/{4}/{0}/plugins/{3}/Views/{2}/{1}.vbhtml",
               "~/Areas/{4}/{0}/plugins/{3}/Views/Shared/{1}.cshtml",
               "~/Areas/{4}/{0}/plugins/{3}/Views/Shared/{1}.vbhtml"}
        End Sub


        Private Shared _emptyLocations As String() = New String() {}
        Protected Overrides Function CreatePartialView(controllerContext As ControllerContext, partialPath As String) As IView

            Return New System.Web.Mvc.RazorView(controllerContext, partialPath, Nothing, False, FileExtensions)
        End Function

        Protected Overrides Function CreateView(controllerContext As ControllerContext, viewPath As String, masterPath As String) As IView
            Dim enableViewstart As Boolean = False
            If String.IsNullOrEmpty(masterPath) Then
                enableViewstart = True
            End If
            Return New System.Web.Mvc.RazorView(controllerContext, viewPath, masterPath, enableViewstart, FileExtensions)
        End Function

        Public Overrides Function FindPartialView(controllerContext As ControllerContext, partialViewName As String, useCache As Boolean) As ViewEngineResult
            Dim searchedLocations As String() = New String() {}
            Dim controllerName As String = controllerContext.RouteData.GetRequiredString("controller")
            Dim AreaName As String = controllerContext.RouteData.DataTokens("area")
            Dim ViewPath As String = GetPath(controllerContext, partialViewName, controllerName, AreaName, searchedLocations)
            If [String].IsNullOrEmpty(ViewPath) Then
                Return New ViewEngineResult(searchedLocations)
            End If
            Return New ViewEngineResult(CreatePartialView(controllerContext, ViewPath), Me)
        End Function

        Public Overrides Function FindView(controllerContext As ControllerContext, viewName As String, masterName As String, useCache As Boolean) As ViewEngineResult
            Dim searchedLocations As String() = New String() {}
            Dim controllerName As String = controllerContext.RouteData.GetRequiredString("controller")
            Dim AreaName As String = controllerContext.RouteData.DataTokens("area")
            Dim ViewPath As String = GetPath(controllerContext, viewName, controllerName, AreaName, searchedLocations)
            If [String].IsNullOrEmpty(ViewPath) Then
                Return New ViewEngineResult(searchedLocations)
            End If
            Return New ViewEngineResult(CreateView(controllerContext, ViewPath, ""), Me)
        End Function

        Private Function GetPath(cntx As ControllerContext, ViewName As String, ControllerName As String, AreaName As String, ByRef searchedLocations As String()) As String
            Dim result As String = [String].Empty
            If String.IsNullOrEmpty(ViewName) Then
                Return result
            End If
            Dim locations As New List(Of ViewLocation)()
            Dim isBackend As Boolean = False
            Dim PluginName As String = ""


            If cntx.Controller.GetType.GetCustomAttributes(GetType(ThemeSettingsAttribute), True).Any Then
                Dim ThemeSettings As ThemeSettingsAttribute = cntx.Controller.GetType.GetCustomAttributes(GetType(ThemeSettingsAttribute), True).FirstOrDefault()
                isBackend = ThemeSettings.isBackend
                PluginName = ThemeSettings.PluginName
            End If

            If Not String.IsNullOrEmpty(AreaName) Then
                isBackend = True
            End If

            If isBackend Then
                If String.IsNullOrEmpty(PluginName) Then
                    For Each viewLocationFormat As String In Me.AreaViewLocationFormats
                        locations.Add(New ViewLocation(viewLocationFormat))
                    Next
                Else
                    For Each viewLocationFormat As String In Me.AreaPluginViewLocationFormats
                        locations.Add(New ViewLocation(viewLocationFormat))
                    Next
                End If
            Else
                If String.IsNullOrEmpty(PluginName) Then
                    For Each viewLocationFormat As String In MyBase.ViewLocationFormats
                        locations.Add(New ViewLocation(viewLocationFormat))
                    Next
                Else
                    For Each viewLocationFormat As String In Me.PluginViewLocationFormats
                        locations.Add(New ViewLocation(viewLocationFormat))
                    Next
                End If

            End If




            searchedLocations = New String(locations.Count - 1) {}
            For i As Integer = 0 To locations.Count - 1
                Dim location As ViewLocation = locations(i)
                Dim virtualPath As String = location.Format(ViewName, ControllerName, PluginName, AreaName)
                Dim virtualPathDisplayInfo As DisplayInfo = DisplayModeProvider.GetDisplayInfoForVirtualPath(virtualPath, cntx.HttpContext, Function(path)
                                                                                                                                                Return FileExists(cntx, path)
                                                                                                                                            End Function, cntx.DisplayMode)
                If virtualPathDisplayInfo IsNot Nothing Then
                    Dim resolvedVirtualPath As String = virtualPathDisplayInfo.FilePath
                    searchedLocations = _emptyLocations
                    result = resolvedVirtualPath

                    If cntx.DisplayMode Is Nothing Then
                        cntx.DisplayMode = virtualPathDisplayInfo.DisplayMode
                    End If
                    Exit For
                End If
                searchedLocations(i) = virtualPath
            Next
            Return result


        End Function
        Protected Overrides Function FileExists(controllerContext As ControllerContext, virtualPath As String) As Boolean
            Try
                Return MyBase.FileExists(controllerContext, virtualPath)
            Catch ex As Exception
                Return False
            End Try
        End Function

    End Class

End Namespace

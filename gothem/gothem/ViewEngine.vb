Imports System.Web.Mvc
Imports System.Web.WebPages

Namespace gozeer.web.mvc


    Public Class ViewEngine
        Inherits VirtualPathProviderViewEngine
        ''' <summary>
        ''' 0 Theme
        ''' 1 Action
        ''' 2 Controller
        ''' 3 Plugin
        ''' 4 Area
        ''' </summary>
        Private Const CacheKeyFormat As String = ":ViewCacheEntry:{0}:{1}:{2}:{3}:{4}"

        Public PluginViewLocationFormats As String()
        Public AreaPluginViewLocationFormats As String()


        Public Sub New()
            MyBase.FileExtensions = New String() {"vbhtml", "cshtml", "aspx"}


            MyBase.ViewLocationFormats = New String() {
                "~/_gozeer/frontend/lookandfeel/{0}/Views/{2}/{1}.aspx",
                "~/_gozeer/frontend/lookandfeel/{0}/Views/{2}/{1}.ascx",
                "~/_gozeer/frontend/lookandfeel/{0}/Views/Shared/{1}.aspx",
                "~/_gozeer/frontend/lookandfeel/{0}/Views/Shared/{1}.ascx",
                "~/_gozeer/frontend/lookandfeel/{0}/Views/{2}/{1}.cshtml",
                "~/_gozeer/frontend/lookandfeel/{0}/Views/{2}/{1}.vbhtml",
                "~/_gozeer/frontend/lookandfeel/{0}/Views/Shared/{1}.cshtml",
                "~/_gozeer/frontend/lookandfeel/{0}/Views/Shared/{1}.vbhtml"
            }
            MyBase.PartialViewLocationFormats = MyBase.ViewLocationFormats
            MyBase.MasterLocationFormats = MyBase.ViewLocationFormats

            MyBase.AreaViewLocationFormats = New String() {
               "~/_gozeer/backend/{4}Views/{2}/{1}.aspx",
               "~/_gozeer/backend/{4}Views/{2}/{1}.ascx",
               "~/_gozeer/backend/{4}Views/Shared/{1}.aspx",
               "~/_gozeer/backend/{4}Views/Shared/{1}.ascx",
               "~/_gozeer/backend/{4}Views/{2}/{1}.cshtml",
               "~/_gozeer/backend/{4}Views/{2}/{1}.vbhtml",
               "~/_gozeer/backend/{4}Views/Shared/{1}.cshtml",
               "~/_gozeer/backend/{4}Views/Shared/{1}.vbhtml"}
            MyBase.AreaPartialViewLocationFormats = MyBase.AreaViewLocationFormats
            MyBase.AreaMasterLocationFormats = MyBase.AreaViewLocationFormats

            Me.PluginViewLocationFormats = New String() {
                "~/_gozeer/frontend/lookandfeel/{0}/plugins/{3}/Views/{2}/{1}.aspx",
                "~/_gozeer/frontend/lookandfeel/{0}/plugins/{3}/Views/{2}/{1}.ascx",
                "~/_gozeer/frontend/lookandfeel/{0}/plugins/{3}/Views/Shared/{1}.aspx",
                "~/_gozeer/frontend/lookandfeel/{0}/plugins/{3}/Views/Shared/{1}.ascx",
                "~/_gozeer/frontend/lookandfeel/{0}/plugins/{3}/Views/{2}/{1}.cshtml",
                "~/_gozeer/frontend/lookandfeel/{0}/plugins/{3}/Views/{2}/{1}.vbhtml",
                "~/_gozeer/frontend/lookandfeel/{0}/plugins/{3}/Views/Shared/{1}.cshtml",
                "~/_gozeer/frontend/lookandfeel/{0}/plugins/{3}/Views/Shared/{1}.vbhtml",
                "~/_gozeer/plugins/{3}/Views/{2}/{1}.aspx",
                "~/_gozeer/plugins/{3}/Views/{2}/{1}.ascx",
                "~/_gozeer/plugins/{3}/Views/Shared/{1}.aspx",
                "~/_gozeer/plugins/{3}/Views/Shared/{1}.ascx",
                "~/_gozeer/plugins/{3}/Views/{2}/{1}.cshtml",
                "~/_gozeer/plugins/{3}/Views/{2}/{1}.vbhtml",
                "~/_gozeer/plugins/{3}/Views/Shared/{1}.cshtml",
                "~/_gozeer/plugins/{3}/Views/Shared/{1}.vbhtml"
            }

            Me.AreaPluginViewLocationFormats = New String() {
               "~/_gozeer/backend/{4}plugins/{3}/Views/{2}/{1}.aspx",
               "~/_gozeer/backend/{4}plugins/{3}/Views/{2}/{1}.ascx",
               "~/_gozeer/backend/{4}plugins/{3}/Views/Shared/{1}.aspx",
               "~/_gozeer/backend/{4}plugins/{3}/Views/Shared/{1}.ascx",
               "~/_gozeer/backend/{4}plugins/{3}/Views/{2}/{1}.cshtml",
               "~/_gozeer/backend/{4}plugins/{3}/Views/{2}/{1}.vbhtml",
               "~/_gozeer/backend/{4}plugins/{3}/Views/Shared/{1}.cshtml",
               "~/_gozeer/backend/{4}plugins/{3}/Views/Shared/{1}.vbhtml",
               "~/_gozeer/plugins/{3}/backend/Views/{2}/{1}.aspx",
               "~/_gozeer/plugins/{3}/backend/Views/{2}/{1}.ascx",
               "~/_gozeer/plugins/{3}/backend/Views/Shared/{1}.aspx",
               "~/_gozeer/plugins/{3}/backend/Views/Shared/{1}.ascx",
               "~/_gozeer/plugins/{3}/backend/Views/{2}/{1}.cshtml",
               "~/_gozeer/plugins/{3}/backend/Views/{2}/{1}.vbhtml",
               "~/_gozeer/plugins/{3}/backend/Views/Shared/{1}.cshtml",
               "~/_gozeer/plugins/{3}/backend/Views/Shared/{1}.vbhtml"}
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


            For Each t In cntx.Controller.GetType.Assembly.GetTypes().Where(Function(tx) GetType(IThemeBaseSettings).IsAssignableFrom(tx)) ' tx.IsClass AndAlso tx.IsAbstract = False) '.Where(Function(tx) tx.IsAssignableFro(GetType(IThemeBaseSettings))) 'And tx.IsInterface = False And (tx.IsClass AndAlso tx.IsAbstract = False))
                If GetType(IThemeBaseSettings).IsAssignableFrom(t) Then
                    If Not t.IsInterface Then
                        If t.IsClass AndAlso Not t.IsAbstract Then
                            Dim Plugin = CType(Activator.CreateInstance(t), IThemeBaseSettings)
                            PluginName = Plugin.PluginName
                            Exit For
                        End If
                    End If
                End If
            Next


            If cntx.Controller.GetType.GetCustomAttributes(GetType(ThemeSettingsAttribute), True).Any Then
                Dim ThemeSettings As ThemeSettingsAttribute = cntx.Controller.GetType.GetCustomAttributes(GetType(ThemeSettingsAttribute), True).FirstOrDefault()
                isBackend = ThemeSettings.isBackend
                If (Not String.IsNullOrEmpty(ThemeSettings.PluginName)) Then
                    PluginName = ThemeSettings.PluginName
                End If

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


            If Not String.IsNullOrEmpty(AreaName) Then
                Dim remArea As String = AreaName
                AreaName = remArea & "/"
            End If

            searchedLocations = New String(locations.Count - 1) {}

            If ViewName.StartsWith("~") Then
                searchedLocations = New String() {ViewName}
            End If
            For i As Integer = 0 To locations.Count - 1
                Dim location As ViewLocation = locations(i)
                Dim virtualPath As String
                If ViewName.StartsWith("~") Then
                    virtualPath = ViewName
                Else
                    virtualPath = location.Format(ViewName, ControllerName, PluginName, AreaName)
                End If

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

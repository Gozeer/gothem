Imports System.Web.Mvc


Public Class ThemeBaseControl
    Inherits Controller
    Implements IThemeBaseControl
    Public Property isBackend As Boolean Implements IThemeBaseControl.isBackend

    Public Property PluginName As String Implements IThemeBaseControl.PluginName
End Class

<ThemeSettings(GetType(IThemeBaseControl))>
Public Interface IThemeBaseControl
    Property PluginName As String
    Property isBackend As Boolean
End Interface

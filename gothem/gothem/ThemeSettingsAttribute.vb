Public Class ThemeSettingsAttribute
    Inherits Attribute
    Public Property PluginName As String
    Public Property isBackend As Boolean
    Public Sub New(Optional PluginName As String = "", Optional isBackend As Boolean = False)
        Me.PluginName = PluginName
        Me.isBackend = isBackend
    End Sub

End Class

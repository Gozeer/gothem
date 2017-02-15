Public Class ThemeSettingsAttribute
    Inherits Attribute
    Public Property PluginName As String
    Public Property isBackend As Boolean
    Public Sub New(Optional PluginName As String = "", Optional isBackend As Boolean = False)
        If (Not String.IsNullOrEmpty(PluginName)) Then
            Me.PluginName = PluginName
        End If
        Me.isBackend = isBackend
    End Sub

    Public Sub New(PluginType As Type, Optional isBackend As Boolean = False)

    End Sub


End Class



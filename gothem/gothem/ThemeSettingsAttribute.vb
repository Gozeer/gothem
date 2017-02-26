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

    Public Sub New(PluginType As Type)
        For Each t In PluginType.Assembly.GetTypes()
            If GetType(IThemeBaseSettings).IsAssignableFrom(t) Then
                If Not t.IsInterface Then
                    If t.IsClass AndAlso Not t.IsAbstract Then
                        Dim Plugin = CType(Activator.CreateInstance(t), IThemeBaseSettings)
                        Me.PluginName = Plugin.PluginName
                        Exit For
                    End If
                End If
            End If
        Next
    End Sub
End Class



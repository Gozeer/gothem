Public Class PluginSettings
    Implements IThemeBaseSettings

    Private _PluginName As String = "DemoPlugin"
    Public Property PluginName As String Implements IThemeBaseSettings.PluginName
        Get
            Return _PluginName
        End Get
        Set(value As String)
            _PluginName = value
        End Set
    End Property
End Class

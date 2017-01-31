Imports System.IO
Imports System.Web.Mvc
Imports System.Web.Routing

Namespace gozeer.web.mvc
    Public Class RazorView
        Inherits System.Web.Mvc.RazorView

        Private requestContrext As RequestContext
        Private controllerContext As ControllerContext
        Public Event ViewRendered(viewContext As ViewContext, writer As TextWriter, ByRef htmlContent As String)

        Public Sub New(controllerContext As ControllerContext, viewPath As String, layoutPath As String, runViewStartPages As Boolean, viewStartFileExtensions As IEnumerable(Of String))
            MyBase.New(controllerContext, viewPath, layoutPath, runViewStartPages, viewStartFileExtensions)
        End Sub

        Public Overrides Sub Render(viewContext As ViewContext, writer As TextWriter)
            MyBase.Render(viewContext, writer)
        End Sub
        Protected Overrides Sub RenderView(viewContext As ViewContext, writer As TextWriter, instance As Object)
            Me.requestContrext = viewContext.RequestContext
            Me.controllerContext = viewContext.Controller.ControllerContext
            Dim sw As New StringWriter
            MyBase.RenderView(viewContext, sw, instance)
            Dim str As String = sw.ToString
            RaiseEvent ViewRendered(viewContext, writer, str)
            writer.Write(str)
        End Sub
    End Class

End Namespace

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.IO;
using System.Web.Routing;
using System.Web;
using System.Web.Hosting;
using System.Web.Compilation;
using System.Web.Caching;
using Ivony.Fluent;

namespace Ivony.Html.Web.Mvc
{
  public abstract class JumonyView : JumonyViewBase
  {
    public JumonyView( string virtualPath, bool isPartial )
    {
      VirtualPath = virtualPath;
      IsPartial = isPartial;
    }



    protected JumonyView()
    {

    }




    protected IHtmlDocument Document
    {
      get;
      private set;
    }


    protected override string RenderContent()
    {
      return Document.Render();
    }


    protected override void ProcessMain()
    {
      Document = LoadDocument();

      ProcessDocument();

      ProcessActionLinks( Document );

      ProcessPartials( Document );

      Document.ResolveUriToAbsoluate();
    }

    protected virtual IHtmlDocument LoadDocument()
    {
      return HtmlProviders.LoadDocument( HttpContext, VirtualPath );
    }


    protected abstract void ProcessDocument();



    private void AddGeneratorMetaData()
    {
      var modifier = Document.DomModifier;
      if ( modifier != null )
      {
        var header = Document.Find( "html head" ).FirstOrDefault();

        if ( header != null )
        {

          var metaElement = modifier.AddElement( header, "meta" );

          metaElement.SetAttribute( "name", "generator" );
          metaElement.SetAttribute( "content", "Jumony" );
        }
      }
    }


  }


  public abstract class JumonyView<T> : JumonyView
  {

    protected new T ViewModel
    {
      get { return base.ViewModel.CastTo<T>(); }
    }

  }


  internal class GenericView : JumonyView
  {

    public GenericView( string virtualPath, bool isPartial )
      : base( virtualPath, isPartial )
    {

    }


    protected override void ProcessDocument()
    {
      return;
    }
  }
}

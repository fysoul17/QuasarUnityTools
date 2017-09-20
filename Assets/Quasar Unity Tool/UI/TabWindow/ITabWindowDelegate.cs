////////////////////////////////////////////////////////////////////////////
// ITabWindowDelegate
//
// Copyright (C) 2014-2016 Terry K. @ GIX Entertainment Inc. & Quasar Inc.
//
////////////////////////////////////////////////////////////////////////////

using UnityEngine;

public interface ITabWindowDelegate
{
    /// <summary>
    /// This will get called regardless of the view's activation state. Use this for performing some action even if it is activated and selected again.
    /// </summary>
    /// <param name="selectedView"></param>
    /// <param name="index"></param>
    void DidSelectTabAtIndex(GameObject selectedView, int index);

    /// <summary>
    /// This only gets called when activated from other tab. Use this if some action should not be performed when selecting activated view again.
    /// </summary>
    /// <param name="view"></param>
    /// <param name="index"></param>
    void DidActivateViewAtIndex(GameObject view, int index);
}

package md546a182019d6511bfce654d61824a1f9f;


public class BrowserTabActivity
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("Microsoft.Identity.Client.BrowserTabActivity, Microsoft.Identity.Client, Version=1.1.0.0, Culture=neutral, PublicKeyToken=0a613f4dd989e8ae", BrowserTabActivity.class, __md_methods);
	}


	public BrowserTabActivity () throws java.lang.Throwable
	{
		super ();
		if (getClass () == BrowserTabActivity.class)
			mono.android.TypeManager.Activate ("Microsoft.Identity.Client.BrowserTabActivity, Microsoft.Identity.Client, Version=1.1.0.0, Culture=neutral, PublicKeyToken=0a613f4dd989e8ae", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}

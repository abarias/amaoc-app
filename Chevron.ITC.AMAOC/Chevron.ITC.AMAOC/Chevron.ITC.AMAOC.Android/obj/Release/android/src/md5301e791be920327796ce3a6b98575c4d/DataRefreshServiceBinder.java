package md5301e791be920327796ce3a6b98575c4d;


public class DataRefreshServiceBinder
	extends android.os.Binder
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Chevron.ITC.AMAOC.Droid.DataRefreshServiceBinder, Chevron.ITC.AMAOC.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", DataRefreshServiceBinder.class, __md_methods);
	}


	public DataRefreshServiceBinder () throws java.lang.Throwable
	{
		super ();
		if (getClass () == DataRefreshServiceBinder.class)
			mono.android.TypeManager.Activate ("Chevron.ITC.AMAOC.Droid.DataRefreshServiceBinder, Chevron.ITC.AMAOC.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public DataRefreshServiceBinder (com.Chevron.ITC.AMAOC.DataRefreshService p0) throws java.lang.Throwable
	{
		super ();
		if (getClass () == DataRefreshServiceBinder.class)
			mono.android.TypeManager.Activate ("Chevron.ITC.AMAOC.Droid.DataRefreshServiceBinder, Chevron.ITC.AMAOC.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Chevron.ITC.AMAOC.Droid.DataRefreshService, Chevron.ITC.AMAOC.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", this, new java.lang.Object[] { p0 });
	}

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

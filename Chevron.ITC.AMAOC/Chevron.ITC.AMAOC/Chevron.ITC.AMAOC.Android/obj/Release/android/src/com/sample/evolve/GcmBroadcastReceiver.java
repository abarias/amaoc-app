package com.sample.evolve;


public class GcmBroadcastReceiver
	extends md5595da4d4e7cb22fe6af1e179e7257f69.GcmBroadcastReceiverBase_1
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Chevron.ITC.AMAOC.Droid.GcmBroadcastReceiver, Chevron.ITC.AMAOC.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", GcmBroadcastReceiver.class, __md_methods);
	}


	public GcmBroadcastReceiver () throws java.lang.Throwable
	{
		super ();
		if (getClass () == GcmBroadcastReceiver.class)
			mono.android.TypeManager.Activate ("Chevron.ITC.AMAOC.Droid.GcmBroadcastReceiver, Chevron.ITC.AMAOC.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
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

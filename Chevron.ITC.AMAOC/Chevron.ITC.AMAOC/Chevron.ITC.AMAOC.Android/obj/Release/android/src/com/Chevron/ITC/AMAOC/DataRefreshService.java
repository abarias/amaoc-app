package com.Chevron.ITC.AMAOC;


public class DataRefreshService
	extends com.google.android.gms.gcm.GcmTaskService
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onBind:(Landroid/content/Intent;)Landroid/os/IBinder;:GetOnBind_Landroid_content_Intent_Handler\n" +
			"n_onInitializeTasks:()V:GetOnInitializeTasksHandler\n" +
			"n_onStartCommand:(Landroid/content/Intent;II)I:GetOnStartCommand_Landroid_content_Intent_IIHandler\n" +
			"n_onRunTask:(Lcom/google/android/gms/gcm/TaskParams;)I:GetOnRunTask_Lcom_google_android_gms_gcm_TaskParams_Handler\n" +
			"";
		mono.android.Runtime.register ("Chevron.ITC.AMAOC.Droid.DataRefreshService, Chevron.ITC.AMAOC.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", DataRefreshService.class, __md_methods);
	}


	public DataRefreshService () throws java.lang.Throwable
	{
		super ();
		if (getClass () == DataRefreshService.class)
			mono.android.TypeManager.Activate ("Chevron.ITC.AMAOC.Droid.DataRefreshService, Chevron.ITC.AMAOC.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public android.os.IBinder onBind (android.content.Intent p0)
	{
		return n_onBind (p0);
	}

	private native android.os.IBinder n_onBind (android.content.Intent p0);


	public void onInitializeTasks ()
	{
		n_onInitializeTasks ();
	}

	private native void n_onInitializeTasks ();


	public int onStartCommand (android.content.Intent p0, int p1, int p2)
	{
		return n_onStartCommand (p0, p1, p2);
	}

	private native int n_onStartCommand (android.content.Intent p0, int p1, int p2);


	public int onRunTask (com.google.android.gms.gcm.TaskParams p0)
	{
		return n_onRunTask (p0);
	}

	private native int n_onRunTask (com.google.android.gms.gcm.TaskParams p0);

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

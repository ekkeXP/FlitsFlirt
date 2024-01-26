package crc64ff0d698ae3fff903;


public class DangerousTrustProvider_DangerousTrustManagerFactory
	extends javax.net.ssl.TrustManagerFactorySpi
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_engineInit:(Ljavax/net/ssl/ManagerFactoryParameters;)V:GetEngineInit_Ljavax_net_ssl_ManagerFactoryParameters_Handler\n" +
			"n_engineInit:(Ljava/security/KeyStore;)V:GetEngineInit_Ljava_security_KeyStore_Handler\n" +
			"n_engineGetTrustManagers:()[Ljavax/net/ssl/TrustManager;:GetEngineGetTrustManagersHandler\n" +
			"";
		mono.android.Runtime.register ("Flitsflirt.App.DangerousTrustProvider+DangerousTrustManagerFactory, Flitsflirt.App", DangerousTrustProvider_DangerousTrustManagerFactory.class, __md_methods);
	}


	public DangerousTrustProvider_DangerousTrustManagerFactory ()
	{
		super ();
		if (getClass () == DangerousTrustProvider_DangerousTrustManagerFactory.class) {
			mono.android.TypeManager.Activate ("Flitsflirt.App.DangerousTrustProvider+DangerousTrustManagerFactory, Flitsflirt.App", "", this, new java.lang.Object[] {  });
		}
	}


	public void engineInit (javax.net.ssl.ManagerFactoryParameters p0)
	{
		n_engineInit (p0);
	}

	private native void n_engineInit (javax.net.ssl.ManagerFactoryParameters p0);


	public void engineInit (java.security.KeyStore p0)
	{
		n_engineInit (p0);
	}

	private native void n_engineInit (java.security.KeyStore p0);


	public javax.net.ssl.TrustManager[] engineGetTrustManagers ()
	{
		return n_engineGetTrustManagers ();
	}

	private native javax.net.ssl.TrustManager[] n_engineGetTrustManagers ();

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

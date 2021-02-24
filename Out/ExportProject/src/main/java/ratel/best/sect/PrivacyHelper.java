package ratel.best.sect;
import android.widget.Toast;

import com.nefarian.privacy.policy.IPrivacyPolicyCallback;
import com.nefarian.privacy.policy.PrivacyPolicyHelper;

import ratel.best.sect.ExActivityListener;
import ratel.best.sect.UnityApplication;
import ratel.best.sect.UnityPlayerActivity;

public class PrivacyHelper{

    private ExActivityListener listener;
    public void setListener(ExActivityListener listener)
    {
        this.listener=listener;
    }

    //创建隐私协议对象
    public PrivacyPolicyHelper getPrivacyHelper()
    {
        PrivacyPolicyHelper privacyPolicyHelper =
                new PrivacyPolicyHelper.Builder(UnityApplication.mUnityActivity).callback(new IPrivacyPolicyCallback() {
                    @Override
                    public void onUserDisagree() {
                        if(listener != null) listener.onDisagree();
                    }

                    @Override
                    public void onUserAgree() {
                        Toast.makeText(UnityApplication.mUnityActivity, "User Agree", Toast.LENGTH_SHORT).show();
                        if(listener != null) listener.onAgree();
                    }
                }).build();
        return privacyPolicyHelper;
    }
}
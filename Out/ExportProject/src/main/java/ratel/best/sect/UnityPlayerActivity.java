package ratel.best.sect;

import android.Manifest;
import android.annotation.TargetApi;
import android.app.Activity;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.content.res.Configuration;
import android.os.Build;
import android.os.Bundle;
import android.view.KeyEvent;
import android.view.MotionEvent;
import android.view.Window;

import androidx.annotation.NonNull;
import androidx.core.app.ActivityCompat;

import com.coala.statisticscore.StatisticsSDK;
import com.taurusx.ads.core.api.TaurusXAds;
import com.taurusx.ads.core.api.utils.LogUtil;
import com.unity3d.player.IUnityPlayerLifecycleEvents;
import com.unity3d.player.UnityPlayer;
import com.we.game.sdk.core.WeGameSdk;

import java.util.ArrayList;
import java.util.List;

public class UnityPlayerActivity extends Activity {
    protected UnityPlayer mUnityPlayer; // don't change the name of this variable; referenced from native code

    // Setup activity layout
    @Override
    protected void onCreate(Bundle savedInstanceState) {

        requestWindowFeature(Window.FEATURE_NO_TITLE);
        super.onCreate(savedInstanceState);

        UnityApplication.mUnityActivity = this;

        mUnityPlayer = new UnityPlayer(this);
        setContentView(mUnityPlayer);
        mUnityPlayer.requestFocus();

        TaurusXAds.getDefault().setLogEnable(true);
    }

    @TargetApi(Build.VERSION_CODES.M)
    private void checkAndRequestPermission() {
        List<String> lackedPermission = new ArrayList<String>();
        if (!(checkSelfPermission(Manifest.permission.READ_PHONE_STATE) == PackageManager.PERMISSION_GRANTED)) {
            lackedPermission.add(Manifest.permission.READ_PHONE_STATE);
        }
        if (!(checkSelfPermission(Manifest.permission.ACCESS_COARSE_LOCATION) == PackageManager.PERMISSION_GRANTED)) {
            lackedPermission.add(Manifest.permission.ACCESS_COARSE_LOCATION);
        }
        if (!(checkSelfPermission(Manifest.permission.WRITE_EXTERNAL_STORAGE) == PackageManager.PERMISSION_GRANTED)) {
            lackedPermission.add(Manifest.permission.WRITE_EXTERNAL_STORAGE);
        }

        // 权限都已经有了，那么直接调用SDK
        if (lackedPermission.size() > 0) {
            // 请求所缺少的权限，在onRequestPermissionsResult中再看是否获得权限，如果获得权限就可以调用SDK，否则不要调用SDK。
            String[] requestPermissions = new String[lackedPermission.size()];
            lackedPermission.toArray(requestPermissions);
            ActivityCompat.requestPermissions(this, requestPermissions, PERMISSION_REQUEST_CODE);
        }
    }
    private final int PERMISSION_REQUEST_CODE = 1024;

    @Override
    public void onRequestPermissionsResult(int requestCode,
                                           @NonNull String[] permissions,
                                           @NonNull int[] grantResults) {
        if(requestCode == PERMISSION_REQUEST_CODE) {
            boolean grantAll = true;
            for (int i = 0; i < permissions.length; i++) {
                String permission = permissions[i];
                if (permission.equals(Manifest.permission.READ_PHONE_STATE)
                        || permission.equals( Manifest.permission.WRITE_EXTERNAL_STORAGE)) {
                    int grantResult = grantResults[i];
                    if (grantResult == PackageManager.PERMISSION_DENIED) {
                        grantAll = false;
                        break;
                    }
                }
            }
            if (grantAll) {
                LogUtil.d("permission", "All permission are granted");
            } else {
                LogUtil.e("permission", "BDManager need permission to granted");
            }
        }
    }

    @Override
    protected void onNewIntent(Intent intent) {
        // To support deep linking, we need to make sure that the client can get access to
        // the last sent intent. The clients access this through a JNI api that allows them
        // to get the intent set on launch. To update that after launch we have to manually
        // replace the intent with the one caught here.
        setIntent(intent);
        WeGameSdk.getInstance().onNewIntent(intent);
        StatisticsSDK.getInstance().onNewIntent(intent);


    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        WeGameSdk.getInstance().onActivityResult(requestCode, resultCode, data);
        StatisticsSDK.getInstance().onActivityResult(requestCode, resultCode, data);
    }

    @Override
    protected void onRestart() {
        super.onRestart();
        WeGameSdk.getInstance().onRestart();
        StatisticsSDK.getInstance().onRestart();
    }

    // Quit Unity
    @Override
    protected void onDestroy() {
        mUnityPlayer.quit();
        super.onDestroy();
        WeGameSdk.getInstance().onDestroy();
        StatisticsSDK.getInstance().onDestroy();
    }

    // Pause Unity
    @Override
    protected void onPause() {
        super.onPause();
        mUnityPlayer.pause();
        WeGameSdk.getInstance().onPause();
        StatisticsSDK.getInstance().onPause();
    }

    // Resume Unity
    @Override
    protected void onResume() {
        super.onResume();
        mUnityPlayer.resume();
        WeGameSdk.getInstance().onResume();
        StatisticsSDK.getInstance().onResume();

/*        if (BuildConfig.BuildChannel.equals("yyb") || BuildConfig.BuildChannel.equals("gdt")) {
            GDTAction.logAction(ActionType.START_APP);
        }*/
    }

    @Override
    protected void onStart() {
        super.onStart();
        //mUnityPlayer.start();
        WeGameSdk.getInstance().onStart();
        StatisticsSDK.getInstance().onStart();
    }

    @Override
    protected void onStop() {
        super.onStop();
        //mUnityPlayer.stop();
        if (isFinishing()) {
            UnityApplication.mUnityActivity = null;
        }
        WeGameSdk.getInstance().onStop();
        StatisticsSDK.getInstance().onStop();
    }

    // Low Memory Unity
    @Override
    public void onLowMemory() {
        super.onLowMemory();
        mUnityPlayer.lowMemory();
    }

    // Trim Memory Unity
    @Override
    public void onTrimMemory(int level) {
        super.onTrimMemory(level);
        if (level == TRIM_MEMORY_RUNNING_CRITICAL) {
            mUnityPlayer.lowMemory();
        }
    }

    // This ensures the layout will be correct.
    @Override
    public void onConfigurationChanged(Configuration newConfig) {
        super.onConfigurationChanged(newConfig);
        mUnityPlayer.configurationChanged(newConfig);
    }

    // Notify Unity of the focus change.
    @Override
    public void onWindowFocusChanged(boolean hasFocus) {
        super.onWindowFocusChanged(hasFocus);
        mUnityPlayer.windowFocusChanged(hasFocus);
    }

    // For some reason the multiple keyevent type is not supported by the ndk.
    // Force event injection by overriding dispatchKeyEvent().
    @Override
    public boolean dispatchKeyEvent(KeyEvent event) {
        if (event.getAction() == KeyEvent.ACTION_MULTIPLE)
            return mUnityPlayer.injectEvent(event);
        return super.dispatchKeyEvent(event);
    }

    // Pass any events not handled by (unfocused) views straight to UnityPlayer
    @Override
    public boolean onKeyUp(int keyCode, KeyEvent event) {
        return mUnityPlayer.injectEvent(event);
    }

    @Override
    public boolean onKeyDown(int keyCode, KeyEvent event) {
        return mUnityPlayer.injectEvent(event);
    }

    @Override
    public boolean onTouchEvent(MotionEvent event) {
        return mUnityPlayer.injectEvent(event);
    }

    /*API12*/
    public boolean onGenericMotionEvent(MotionEvent event) {
        return mUnityPlayer.injectEvent(event);
    }

/*    //创建隐私协议对象
    public PrivacyPolicyHelper getPrivacyHelper()
    {
        PrivacyPolicyHelper privacyPolicyHelper =
                new PrivacyPolicyHelper.Builder(this).callback(new IPrivacyPolicyCallback() {
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

    private ExActivityListener listener;
    public void setListener(ExActivityListener listener)
    {
        this.listener=listener;
    }*/


}

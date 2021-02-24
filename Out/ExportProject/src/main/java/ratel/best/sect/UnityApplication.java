package ratel.best.sect;

import android.app.Activity;
import android.app.Application;
import android.content.Context;
import android.content.res.Configuration;

import androidx.multidex.MultiDex;

import com.coala.statisticscore.StatisticsApplication;
import com.we.game.sdk.core.WeGameApplication;

/**
 * Created by Administrator on 2019/5/5.
 */

public class UnityApplication extends Application {

    public static Activity mUnityActivity;

    @Override
    public void onCreate() {

        super.onCreate();
        WeGameApplication.onCreate(this);
//        XGPushConfig.enableOtherPush(getApplicationContext(), true);
//        XGPushManager.registerPush(this, new XGIOperateCallback() {
//            @Override
//            public void onSuccess(Object data, int flag) {
//            //token在设备卸载重装的时候有可能会变
//                Log.d("TPush", "注册成功，设备token为：" + data);
//            }
//            @Override
//            public void onFail(Object data, int errCode, String msg) {
//                Log.d("TPush", "注册失败，错误码：" + errCode + ",错误信息：" + msg);
//            }
//        });
//        XGPushManager.setTag(this,"XINGE");

        // StatisticsApplication.onCreate(this);
        //Bugly.init(getApplicationContext(), "d662503942", false);




//        if(BuildConfig.BuildChannel.equals("bytedance_inner")){
//            TaurusXAds.getDefault().setSegment(Segment.Builder().setChannel("bytedance_inner").build());
//        }
        //for gdt
//        else if(BuildConfig.BuildChannel.equals("gdt")){
//            GDTAction.init(this, "1110570922", "834b09f4e80030e00feb8d4e90e66a30");
//        }

        //if(BuildConfig.BuildChannel.equals("oppo"))
        //{
         //   GameCenterSDK.init("35494c4500dd405999154294b21def7d",this);
        //}
    }

    @Override
    public void onTerminate() {
        super.onTerminate();
        WeGameApplication.onTerminate();
        StatisticsApplication.onTerminate();
    }
    @Override
    protected void attachBaseContext(Context base) {
        super.attachBaseContext(base);
        MultiDex.install(this);
        WeGameApplication.attachBaseContext(base);
        StatisticsApplication.attachBaseContext(base);
    }

    @Override
    public void onConfigurationChanged(Configuration newConfig) {
        super.onConfigurationChanged(newConfig);
        WeGameApplication.onConfigurationChanged(newConfig);
        StatisticsApplication.onConfigurationChanged(newConfig);
    }
}

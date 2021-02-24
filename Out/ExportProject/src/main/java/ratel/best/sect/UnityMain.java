package ratel.best.sect;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;

public class UnityMain extends Activity {

    @Override

    protected void onCreate(Bundle savedInstanceState) {

        super.onCreate(savedInstanceState);

        if (UnityApplication.mUnityActivity != null) { //这里是判断应用的activity是否存在，存在的话直接结束这里，否则启动应用

            finish();

        } else {

            Intent intent = new Intent(this, UnityPlayerActivity.class);

            startActivity(intent);

            finish();

        }

    }

}
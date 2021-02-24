#!/bin/bash
#1. Export unity project to android project

repo_name="simswordsman"
branch_name="SimSwordsManRelease"
project_name="SimSwordsman"
product_name="最强门派"

# 0. Activate unity
# xvfb-run --auto-servernum --server-args='-screen 0 640x480x24' /opt/Unity/Editor/Unity \
# -quit \
# -batchmode \
# -logFile /dev/stdout \
# -nographics \
# -manualLicenseFile /project/$repo_name/Unity_lic.ulf

# chmod -R 777 /project/$repo_name/Tools/table
# /bin/sh /project/$repo_name/Tools/table/output_txt_unix.sh
# /bin/sh /project/$repo_name/UnityProject/$project_name/Assets/Creditest/Tables/table/output_txt_unix.sh
# rm -rf /export/android/*

# u3d run -u 2019.3.11f1 -- -batchmode \
# -projectPath /project/$repo_name/UnityProject/$project_name \
# -quit \
# -buildTarget Android \
# -executeMethod "GameBuilder.ProjectBuild.BuildForAndroid" \
# -logFile /project/editor.log \
# -username "378294113@qq.com" \
# -password "198624" \
# -serial "SC-PYFX-89ER-QE96-QUHW-KEK8" \
# -nographics

# cp -f /export/android/local.properties /project/$repo_name/Out/ExportProject/
# cp -r /export/android/unityLibrary/src/main/assets/ /project/$repo_name/Out/ExportProject/src/main/
# cp -f /export/android/unityLibrary/libs/unity-classes.jar /project/$repo_name/Out/ExportProject/libs/
# cp -r /export/android/unityLibrary/src/main/jniLibs/ /project/$repo_name/Out/ExportProject/src/main/
# cp -r /export/android/unityLibrary/src/main/res/values /project/$repo_name/Out/ExportProject/src/main/res/

#2. Invoke gradle to build
chmod -R 777 /project/$repo_name/Out/ExportProject
cd /project/$repo_name/Out/ExportProject
gradle clean
gradle assembleRelease

#3. Copy results to /output
rm -rf /output/*
mkdir -p /output
find /project/ -maxdepth 10 -type f -name "*.apk" | xargs cp -v -t /output
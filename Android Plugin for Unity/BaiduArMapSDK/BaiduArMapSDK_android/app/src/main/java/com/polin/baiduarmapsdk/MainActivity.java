package com.polin.baiduarmapsdk;

import android.app.Activity;
import android.content.Intent;
import android.net.Uri;
import android.provider.Settings;
import android.os.Bundle;
import android.widget.Toast;

import com.baidu.location.BDLocation;
import com.baidu.mapapi.CoordType;
import com.baidu.mapapi.SDKInitializer;
import com.baidu.mapapi.model.LatLng;
import com.baidu.mapapi.search.core.CityInfo;
import com.baidu.mapapi.search.core.PoiInfo;
import com.baidu.mapapi.search.core.SearchResult;
import com.baidu.mapapi.search.poi.OnGetPoiSearchResultListener;
import com.baidu.mapapi.search.poi.PoiDetailResult;
import com.baidu.mapapi.search.poi.PoiDetailSearchResult;
import com.baidu.mapapi.search.poi.PoiIndoorResult;
import com.baidu.mapapi.search.poi.PoiResult;
import com.baidu.mapapi.search.poi.PoiSearch;
import com.polin.baiduarmapsdk.utils.LocSdkClient;
import com.polin.baiduarmapsdk.utils.PermissionsChecker;
import com.unity3d.player.UnityPlayerActivity;
import com.baidu.mapsdkplatform.comapi.*;
import java.util.ArrayList;
import java.util.List;

import map.baidu.ar.init.ArBuildingResponse;
import map.baidu.ar.init.ArSceneryResponse;
import map.baidu.ar.init.ArSdkManager;
import map.baidu.ar.init.MKGeneralListener;
import map.baidu.ar.init.OnGetDataResultListener;
import map.baidu.ar.model.ArInfoScenery;
import map.baidu.ar.model.ArLatLng;
import map.baidu.ar.model.ArPoiInfo;
import map.baidu.ar.model.PoiInfoImpl;
import map.baidu.ar.utils.ArBDLocation;

import com.polin.baiduarmapsdk.utils.ConstantValue;

public class MainActivity extends UnityPlayerActivity implements OnGetDataResultListener, OnGetPoiSearchResultListener{

    public static ArInfoScenery arInfoScenery; // 景区
    public static ArBuildingResponse arBuildingResponse; // 识楼
    public static List<PoiInfoImpl> poiInfos; // 探索
    private PoiSearch mPoiSearch = null;
    private ArSdkManager mArSdkManager = null;
    private LatLng center = new LatLng(22.866084, 108.285501);
    int radius = 500; // 500米半径
    private int loadIndex = 0;
    // 自定义多点数据
    private ArLatLng[] latLngs = {
            new ArLatLng(22.865926, 108.284935),
            new ArLatLng(22.866817, 108.286763),
            new ArLatLng(22.866484, 108.287801),
            new ArLatLng(22.866531, 108.285363),
            new ArLatLng(22.863861, 108.285308)
    };

    // MainActivity初始化函数
    public void init(){
        // ArSDK模块初始化
        ArSdkManager.getInstance().initApplication(this.getApplication(), new MyGeneralListener());

        // 若用百度定位sdk,需要在此初始化定位SDK
        LocSdkClient.getInstance(this).getLocationStart();

        // 若用探索功能需要再这集成检索模块 在使用 SDK 各组间之前初始化 context 信息，传入 ApplicationContext
        SDKInitializer.initialize(this.getApplicationContext());
        // 检索模块 自4.3.0起，百度地图SDK所有接口均支持百度坐标和国测局坐标，用此方法设置您使用的坐标类型.
        // 包括BD09LL和GCJ02两种坐标，默认是BD09LL坐标。
        SDKInitializer.setCoordType(CoordType.BD09LL);

        // 如果需要检索，初始化搜索模块，注册搜索事件监听
        mPoiSearch = PoiSearch.newInstance();
        mPoiSearch.setOnGetPoiSearchResultListener(this);
        // 如果需要Ar景区功能、Ar识楼功能要注册监听
        mArSdkManager = ArSdkManager.getInstance();
        mArSdkManager.setOnGetDataResultListener(this);

        // 判断权限
        PermissionsChecker permissionsChecker = new PermissionsChecker(this);
        if (permissionsChecker.lacksPermissions()) {
            Toast.makeText(this, "缺少权限，请开启权限！", Toast.LENGTH_LONG).show();
            openSetting();
        }
        else{
            Toast.makeText(this, "权限已开启！", Toast.LENGTH_LONG).show();
        }
    }

    /**
     * 打开设置权限界面
     *
     * @param
     */
    public void openSetting() {
        Intent intent = new Intent();
        intent.setAction(Settings.ACTION_APPLICATION_DETAILS_SETTINGS);
        Uri uri = Uri.fromParts("package", getPackageName(), null);
        intent.setData(uri);
        startActivity(intent);
    }

    public void onClick(String index){
        switch (index){
            // 单点数据展示
            case ConstantValue.singlePointShow:
                poiInfos = new ArrayList<PoiInfoImpl>();
                ArPoiInfo poiInfo = new ArPoiInfo();
                ArLatLng arLatLng = new ArLatLng(22.8600997924805, 108.278999328613);
                PoiInfoImpl poiImpl = new PoiInfoImpl();
                poiInfo.name = ConstantValue.exampleLocName;
                poiInfo.location = arLatLng;
                poiImpl.setPoiInfo(poiInfo);
                poiInfos.add(poiImpl);
                Intent intent2SinglePoint = new Intent();
                intent2SinglePoint.setClass(this,ArActivity.class);
                this.startActivity(intent2SinglePoint);
                break;
            // 多点数据展示
            case ConstantValue.multiPointsShow:
                poiInfos = new ArrayList<PoiInfoImpl>();
                int i = 0;
                for (ArLatLng all : latLngs) {
                    ArPoiInfo pTest = new ArPoiInfo();
                    pTest.name = "testPoint" + i++;
                    pTest.location = all;
                    PoiInfoImpl poiImplT = new PoiInfoImpl();
                    poiImplT.setPoiInfo(pTest);
                    poiInfos.add(poiImplT);
                }
                Intent intent2MultiPoint = new Intent();
                intent2MultiPoint.setClass(this,ArActivity.class);
                this.startActivity(intent2MultiPoint);
                break;
            // 景区功能 传入uid信息
            case ConstantValue.sceneryArShow:
                break;
            // 识楼功能
            case ConstantValue.buildingArShow:
                break;
            // 探索功能
            case ConstantValue.exploreArShow:
                break;
            default:
                break;
        }
    }

    static class MyGeneralListener implements MKGeneralListener {
        // 1、事件监听，用来处理通常的网络错误，授权验证错误等
        @Override
        public void onGetPermissionState(int iError) {
            // 2、非零值表示key验证未通过
            if (iError != 0) {
                // 授权Key错误：
                Toast.makeText(GetActivity(),"arsdk 验证异常，请在AndoridManifest.xml中输入正确的授权Key,并检查您的网络连接是否正常！error: " + iError, Toast
                                .LENGTH_LONG).show();
            }
            else {
                Toast.makeText(GetActivity(), "key认证成功", Toast.LENGTH_LONG).show();
            }
        }

        // 回调给ArSDK获取坐标（demo调用百度定位sdk）
        @Override
        public ArBDLocation onGetBDLocation() {
            // 3、用于传递给ArSdk经纬度信息
            // a、首先通过百度地图定位SDK获取经纬度信息
            // b、包装经纬度信息到ArSdk的ArBDLocation类中 return即可
            BDLocation location =
                    LocSdkClient.getInstance(ArSdkManager.getInstance().getAppContext()).getLocationStart()
                            .getLastKnownLocation();
            if (location == null) {
                return null;
            }
            ArBDLocation arBDLocation = new ArBDLocation();
            // 设置经纬度信息
            arBDLocation.setLongitude(location.getLongitude());
            arBDLocation.setLatitude(location.getLatitude());
            return arBDLocation;
        }
    }

    @Override
    public void onGetPoiResult(PoiResult result) {
        if (result == null || result.error == SearchResult.ERRORNO.RESULT_NOT_FOUND) {
            Toast.makeText(this, "未找到结果", Toast.LENGTH_LONG)
                    .show();
            return;
        }
        if (result.error == SearchResult.ERRORNO.NO_ERROR) {
            poiInfos = new ArrayList<PoiInfoImpl>();
            for (PoiInfo poi : result.getAllPoi()) {
                ArPoiInfo poiInfo = new ArPoiInfo();
                ArLatLng arLatLng = new ArLatLng(poi.location.latitude, poi.location.longitude);
                poiInfo.name = poi.name;
                poiInfo.location = arLatLng;
                PoiInfoImpl poiImpl = new PoiInfoImpl();
                poiImpl.setPoiInfo(poiInfo);
                poiInfos.add(poiImpl);
            }
            Toast.makeText(this, "查询到: " + poiInfos.size() + " ,个POI点", Toast.LENGTH_SHORT).show();
            Intent intent = new Intent(MainActivity.this, ArActivity.class);
            MainActivity.this.startActivity(intent);
            return;
        }
        if (result.error == SearchResult.ERRORNO.AMBIGUOUS_KEYWORD) {

            // 当输入关键字在本市没有找到，但在其他城市找到时，返回包含该关键字信息的城市列表
            String strInfo = "在";
            for (CityInfo cityInfo : result.getSuggestCityList()) {
                strInfo += cityInfo.city;
                strInfo += ",";
            }
            strInfo += "找到结果";
            Toast.makeText(this, strInfo, Toast.LENGTH_LONG)
                    .show();
        }
    }

    @Override
    public void onGetPoiDetailResult(PoiDetailResult result) {
        if (result.error != SearchResult.ERRORNO.NO_ERROR) {
            Toast.makeText(this, "抱歉，未找到结果", Toast.LENGTH_SHORT)
                    .show();
        } else {
            Toast.makeText(this, result.getName() + ": " + result.getAddress(), Toast.LENGTH_SHORT)
                    .show();
        }
    }

    @Override
    public void onGetPoiDetailResult(PoiDetailSearchResult poiDetailSearchResult) {}

    @Override
    public void onGetPoiIndoorResult(PoiIndoorResult poiIndoorResult) {}

    @Override
    public void onGetSceneryResult(ArSceneryResponse var1){}

    @Override
    public void onGetBuildingResult(ArBuildingResponse var1){}

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
    }

    @Override
    protected void onRestart() {
        super.onRestart();
    }

    @Override
    protected void onResume() {
        super.onResume();
    }

    @Override
    protected void onPause() {
        super.onPause();
    }

    @Override
    protected void onStop() {
        super.onStop();
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
    }
    @Override
    protected void onNewIntent(Intent intent) {
        super.onNewIntent(intent);
    }

    /**
     * unity项目启动时的的上下文
     */
    private static Activity _unityActivity;
    /**
     * 获取unity项目的上下文
     * @return
     */
    public static Activity GetActivity(){
        if(null == _unityActivity) {
            try {
                Class<?> classtype = Class.forName("com.unity3d.player.UnityPlayer");
                Activity activity = (Activity) classtype.getDeclaredField("currentActivity").get(classtype);
                _unityActivity = activity;
            } catch (ClassNotFoundException e) {

            } catch (IllegalAccessException e) {

            } catch (NoSuchFieldException e) {

            }
        }
        return _unityActivity;
    }

}

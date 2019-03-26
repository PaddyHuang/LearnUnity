using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour {

    // 获取墙和子弹的纹理图片
    public Texture2D bulletTexture, wallTexture;

    // 定义一个墙面图片的副本
    private Texture2D NewWallTexture;

    // 记录墙面和子弹的宽高
    private float wall_height, wall_width;
    private float bullet_height, bullet_width;

    // 射线焦点信息，用来模拟子弹
    private RaycastHit hit;

    // 定义一个泛型队列来存储像素点的信息
    private Queue<Vector2> uvQueues;

	// Use this for initialization
	void Start () {
        uvQueues = new Queue<Vector2>();
        // 获取墙贴图
        wallTexture = GetComponent<MeshRenderer>().material.mainTexture as Texture2D;
        // 备份一个墙的图片
        NewWallTexture = Instantiate(wallTexture);
        // 将备份赋值回去，作为修改的对象
        GetComponent<MeshRenderer>().material.mainTexture = NewWallTexture;

        wall_height = NewWallTexture.height;
        wall_width = NewWallTexture.width;

        bullet_height = bulletTexture.height;
        bullet_width = bulletTexture.width;

    }
	
	// Update is called once per frame
	void Update () {
        // 鼠标左键发射射线
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                // 判断是否击中墙面
                if (hit.collider.name == "Plane")
                {
                    Vector2 uv = hit.textureCoord;
                    uvQueues.Enqueue(uv);

                    // 遍历子弹图片的区域
                    for (int i = 0; i < bullet_width; i++)
                    {
                        for (int j = 0; j < bullet_height; j++)
                        {
                            // 得到墙壁上对应子弹应该出现的每一个像素点
                            float w = uv.x * wall_width - bullet_width / 2 + i;
                            float h = uv.y * wall_height - bullet_height / 2 + j;

                            // 获取墙上每一个像素点的颜色
                            Color wallColor = NewWallTexture.GetPixel((int)w, (int)h);
                            // 获取子弹的对应的每一个像素点的颜色
                            Color bulletColor = bulletTexture.GetPixel(i, j);
                            // 融合
                            NewWallTexture.SetPixel((int)w, (int)h, wallColor * bulletColor);
                        }
                    }
                    // 将修改的融合图片应用
                    NewWallTexture.Apply();
                    // 3s后指向ReturnWall方法，是墙上的弹痕消失
                    Invoke("ReturnWall", 3);
                }
            }
        }
	}

    // 返回之前的状态
    private void ReturnWall()
    {
        // 获取队列中要出队列的弹痕中心点
        Vector2 uv = uvQueues.Dequeue();

        for (int i = 0; i < bullet_width; i++)
        {
            for (int j = 0; j < bullet_height; j++)
            {
                float w = uv.x * wall_width - bullet_width / 2 + i;
                float h = uv.y * wall_height - bullet_height / 2 + j;

                //获取原墙上的像素点
                Color wallColor = wallTexture.GetPixel((int)w, (int)h);
                NewWallTexture.SetPixel((int)w, (int)h, wallColor);
            }
        }

        NewWallTexture.Apply();
    }

}

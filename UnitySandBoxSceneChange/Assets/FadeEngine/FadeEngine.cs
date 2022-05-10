

/** FadeEngine
*/
namespace FadeEngine
{
	/** FadeEngine
	*/
	public sealed class FadeEngine : System.IDisposable
	{
		/** cell
		*/
		private UnityEngine.Mesh cell_mesh;
		private	int cell_w = 16;
		private int cell_h = 9;
		private UnityEngine.Vector2 cell_size;

		/** layerindex
		*/
		private int layerindex;

		/** camera
		*/
		private UnityEngine.Camera camera_raw;
		private UnityEngine.GameObject camera_gameobject;

		/** canvas
		*/
		private UnityEngine.GameObject canvas_gameobject;

		/** drawinstance
		*/
		private BlueBack.DrwaInstance.DrawInstance drawinstance;
		private BlueBack.DrwaInstance.Buffer<ShaderParam_TypeA_Status> drawinstance_buffer;

		/** フェード。
		*/
		private UnityEngine.Material fade_material;
		private UnityEngine.MaterialPropertyBlock fade_material_propertyblock;
		private float fade_material_move;

		/** ローディング。
		*/
		private UnityEngine.Material loading_material;

		/** screenshot_texture
		*/
		private UnityEngine.Texture2D screenshot_texture;

		/** scenechagnge
		*/
		private float scenechagnge_fadein_time;

		/** constructor
		*/
		public FadeEngine(in InitParam a_initparam)
		{
			//cell
			{
				this.cell_mesh = new UnityEngine.Mesh();
				this.cell_mesh.vertices = new UnityEngine.Vector3[]{
					new UnityEngine.Vector3(0,0,0),
					new UnityEngine.Vector3(1,0,0),
					new UnityEngine.Vector3(1,1,0),
					new UnityEngine.Vector3(0,1,0),
				};
				this.cell_mesh.uv = new UnityEngine.Vector2[]{
					new UnityEngine.Vector3(0,0),
					new UnityEngine.Vector3(1,0),
					new UnityEngine.Vector3(1,1),
					new UnityEngine.Vector3(0,1),
				};
				this.cell_mesh.triangles = new int[]{0,1,2,2,3,0};
				this.cell_mesh.RecalculateNormals();
				this.cell_mesh.RecalculateBounds();
				this.cell_mesh.RecalculateTangents();

				this.cell_w = a_initparam.cell_w;
				this.cell_h = a_initparam.cell_h;
				this.cell_size = new UnityEngine.Vector2(1.0f / this.cell_w,1.0f / this.cell_h);
			}

			//layerindex
			this.layerindex = a_initparam.layerindex;

			//camera
			{
				this.camera_gameobject = new UnityEngine.GameObject("fade_camera");
				UnityEngine.GameObject.DontDestroyOnLoad(this.camera_gameobject);
				this.camera_raw = this.camera_gameobject.AddComponent<UnityEngine.Camera>();
				this.camera_raw.clearFlags = UnityEngine.CameraClearFlags.Nothing;
				this.camera_raw.orthographic = true;
				this.camera_raw.depth = a_initparam.cameradepth;
				this.camera_raw.cullingMask = 1 << this.layerindex;
				this.camera_gameobject.transform.position = new UnityEngine.Vector3(0.0f,0.0f,-5.0f);
			}

			//canvas
			{
				this.canvas_gameobject = UnityEngine.GameObject.Instantiate(a_initparam.canvas_prefab);
				UnityEngine.GameObject.DontDestroyOnLoad(this.canvas_gameobject);
				this.canvas_gameobject.name = "fade_canvas";
				this.canvas_gameobject.SetActive(false);
			}

			//drawinstance
			{
				this.drawinstance = new BlueBack.DrwaInstance.DrawInstance(this.cell_mesh);
				this.drawinstance_buffer = new BlueBack.DrwaInstance.Buffer<ShaderParam_TypeA_Status>(this.cell_w * this.cell_h,32);
			}

			//fade
			{
				this.fade_material = a_initparam.fade_material;

				//custom_matrix
				UnityEngine.Matrix4x4 t_matrix;
				{
					float t_scale = 1.0f / this.cell_h;
					float t_aspect = (float)UnityEngine.Screen.height / UnityEngine.Screen.width;
					t_matrix = new UnityEngine.Matrix4x4(
						new UnityEngine.Vector4(t_aspect * t_scale * 2,0.0f,0.0f,0.0f),
						new UnityEngine.Vector4(0.0f,t_scale * 2,0.0f,0.0f),
						new UnityEngine.Vector4(0.0f,0.0f,t_scale * 2,0.0f),
						new UnityEngine.Vector4(-1.0f,-1.0f,0.0f,1.0f)
					);
				}

				//fade_material_propertyblock
				this.fade_material_propertyblock = new UnityEngine.MaterialPropertyBlock();
				this.fade_material_propertyblock.SetMatrix("custom_matrix",t_matrix);
				this.fade_material_propertyblock.SetFloat("cell_w",this.cell_size.x);
				this.fade_material_propertyblock.SetFloat("cell_h",this.cell_size.y);
				this.fade_material_propertyblock.SetFloat("move",0.0f);
				this.fade_material_propertyblock.SetTexture("_MainTex",UnityEngine.Texture2D.whiteTexture);

				//fade_material_move
				this.fade_material_move = 0.0f;
			}

			//loading
			{
				this.loading_material = new UnityEngine.Material(a_initparam.loading_material);
				this.canvas_gameobject.transform.Find("Loading").GetComponent<UnityEngine.UI.Text>().material = this.loading_material;
			}

			//screenshot
			{
				this.screenshot_texture = null;
			}
		}

		/** [System.IDisposable]破棄。
		*/
		public void Dispose()
		{
			if(this.cell_mesh != null){
				this.cell_mesh.Clear();
				this.cell_mesh = null;
			}

			if(this.camera_gameobject != null){
				UnityEngine.GameObject.DestroyImmediate(this.camera_gameobject);
				this.camera_gameobject = null;
			}

			if(this.canvas_gameobject != null){
				UnityEngine.GameObject.DestroyImmediate(this.canvas_gameobject);
				this.canvas_gameobject = null;
			}

			if(this.drawinstance != null){
				this.drawinstance.Dispose();
				this.drawinstance = null;
			}

			if(this.drawinstance_buffer != null){
				this.drawinstance_buffer.Dispose();
				this.drawinstance_buffer = null;
			}

			if(this.fade_material != null){
				UnityEngine.Resources.UnloadAsset(this.fade_material);
				this.fade_material = null;
			}

			if(this.fade_material_propertyblock != null){
				this.fade_material_propertyblock.Clear();
				this.fade_material_propertyblock = null;
			}

			if(this.screenshot_texture != null){
				UnityEngine.Object.DestroyImmediate(this.screenshot_texture);
				this.screenshot_texture = null;
			}
		}

		/** SceneChangeFadeInStart
		*/
		public void SceneChangeFadeInStart()
		{
			this.scenechagnge_fadein_time = UnityEngine.Time.realtimeSinceStartup;
		}

		/** SceneChangeFadeOutUpdate
		*/
		public void SceneChangeFadeOutUpdate()
		{
			this.Draw();
		}

		/** SceneChangeFadeInUpdate
		*/
		public bool SceneChangeFadeInUpdate()
		{
			if(this.screenshot_texture == null){
				return false;
			}

			this.fade_material_move = (UnityEngine.Time.realtimeSinceStartup - this.scenechagnge_fadein_time);

			if(this.fade_material_move >= 2.5f){
				this.SetTexture(null);
				return false;
			}else{
				this.Draw();
				return true;
			}
		}

		/** フェード経過時間。設定。
		*/
		public void SetFadeMove(float a_move)
		{
			this.fade_material_move = a_move;
		}

		/** テクスチャー。設定。
		*/
		public void SetTexture(UnityEngine.Texture2D a_texture)
		{
			if(a_texture == null){
				UnityEngine.Object.DestroyImmediate(this.screenshot_texture);
				this.screenshot_texture = null;
				this.fade_material_propertyblock.SetTexture("_MainTex",UnityEngine.Texture2D.whiteTexture);
			}else{
				this.screenshot_texture = a_texture;
				this.fade_material_propertyblock.SetTexture("_MainTex",a_texture);
			}
		}

		/** 描画。
		*/
		public void Draw()
		{
			if(this.screenshot_texture != null){
				this.fade_material_propertyblock.SetFloat("move",this.fade_material_move);

				int t_drawcount = this.cell_w * this.cell_h;

				//drawinstance_buffer
				{
					for(int yy=0;yy<this.cell_h;yy++){
						for(int xx=0;xx<this.cell_w;xx++){
							int t_index = yy * this.cell_w + xx;
							this.drawinstance_buffer.raw[t_index] = new ShaderParam_TypeA_Status(
								new UnityEngine.Vector2(xx,yy),
								new UnityEngine.Vector2(xx * this.cell_size.x,yy * this.cell_size.y),
								new UnityEngine.Vector4(1.0f,1.0f,1.0f,1.0f)
							);
						}
					}
					this.drawinstance_buffer.Apply(this.fade_material,"status");
				}

				this.drawinstance.Draw(this.fade_material,this.fade_material_propertyblock,this.camera_raw,this.layerindex,t_drawcount,0);
			}

			//ＵＩ表示。
			if(this.fade_material_move <= 1.5f){
				if(this.screenshot_texture != null){
					this.canvas_gameobject.SetActive(true);
					this.loading_material.SetFloat("move",UnityEngine.Time.realtimeSinceStartup * 5);
				}else{
					this.canvas_gameobject.SetActive(false);
				}
			}else{
				this.canvas_gameobject.SetActive(false);
			}
		}
	}
}


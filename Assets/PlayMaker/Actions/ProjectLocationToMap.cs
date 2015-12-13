// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{	
	[ActionCategory(ActionCategory.Device)]
	[Tooltip("Projects the location found with Get Location Info to a 2d map using common projections.")]
	public class ProjectLocationToMap : FsmStateAction
	{
		// TODO: more projections
		public enum MapProjection
		{
			EquidistantCylindrical,
			Mercator
		}

		[Tooltip("Location vector in degrees longitude and latitude. Typically returned by the Get Location Info action.")]
		public FsmVector3 GPSLocation;
		
		[Tooltip("The projection used by the map.")]
		public MapProjection mapProjection;
		
		[ActionSection("Map Region")]

		//TODO: FsmRect screen region

		[HasFloatSlider(-180,180)]
		public FsmFloat minLongitude;
		
		[HasFloatSlider(-180,180)]
		public FsmFloat maxLongitude;
		
		[HasFloatSlider(-90,90)]
		public FsmFloat minLatitude;
		
		[HasFloatSlider(-90,90)]
		public FsmFloat maxLatitude;
		
		[ActionSection("Screen Region")]

		//TODO: FsmRect screen region

		public FsmFloat minX;
		public FsmFloat minY;
		public FsmFloat width;
		public FsmFloat height;
		
		[ActionSection("Projection")]
		
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the projected X coordinate in a Float Variable. Use this to display a marker on the map.")]
		public FsmFloat projectedX;
		
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the projected Y coordinate in a Float Variable. Use this to display a marker on the map.")]
		public FsmFloat projectedY;
		
		[Tooltip("If true all coordinates in this action are normalized (0-1); otherwise coordinates are in pixels.")]
		public FsmBool normalized;
		
		public bool everyFrame;
		
		private float x,y;
		
		public override void Reset()
		{
			GPSLocation = new FsmVector3 { UseVariable = true };
			mapProjection = MapProjection.EquidistantCylindrical;
			
			minLongitude = -180f;
			maxLongitude = 180f;
			minLatitude = -90f;
			maxLatitude = 90f;
			
			minX = 0;
			minY = 0;
			width = 1;
			height = 1;
			normalized = true;
			
			projectedX = null;
			projectedY = null;
			
			everyFrame = false;
		}

		public override void OnEnter()
		{
			if (GPSLocation.IsNone)
			{
				Finish();
				return;
			}
			
			DoProjectGPSLocation();

			if (!everyFrame)
			{
				Finish();
			}
		}
		
		public override void OnUpdate()
		{
			DoProjectGPSLocation();
		}
		
		void DoProjectGPSLocation()
		{
			x = Mathf.Clamp(GPSLocation.Value.x, minLongitude.Value, maxLongitude.Value);
			y = Mathf.Clamp(GPSLocation.Value.y, minLatitude.Value, maxLatitude.Value);
			
			// projection methods should produce normalized coordinates inside the map region
			
			switch (mapProjection) 
			{
			case MapProjection.EquidistantCylindrical:
				DoEquidistantCylindrical();
				break;
				
			case MapProjection.Mercator:
				DoMercatorProjection();
				break;
			}
			
			x *= width.Value;
			y *= height.Value;
			
			projectedX.Value = normalized.Value ? minX.Value + x : minX.Value + x * Screen.width;
			projectedY.Value = normalized.Value ? minY.Value + y : minY.Value + y * Screen.height;
		}
		
		void DoEquidistantCylindrical()
		{
			x = (x - minLongitude.Value) / (maxLongitude.Value - minLongitude.Value);
			y = (y - minLatitude.Value) / (maxLatitude.Value - minLatitude.Value);
		}
		
		void DoMercatorProjection()
		{
			x = (x - minLongitude.Value) / (maxLongitude.Value - minLongitude.Value);

			var minYProjected = LatitudeToMercator(minLatitude.Value);
			var maxYProjected = LatitudeToMercator(maxLatitude.Value);
			
			y = (LatitudeToMercator(GPSLocation.Value.y) - minYProjected) / (maxYProjected - minYProjected);
		}

		static float LatitudeToMercator(float latitudeInDegrees)
		{
			var lat = Mathf.Clamp(latitudeInDegrees, -85, 85);
			lat = Mathf.Deg2Rad * lat;
    		return Mathf.Log(Mathf.Tan(lat / 2f + Mathf.PI / 4f));
		}
	}
}
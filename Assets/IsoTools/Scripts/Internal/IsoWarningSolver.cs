using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_5_5_OR_NEWER
using UnityEngine.Profiling;
#endif

namespace IsoTools.Internal {
	public class IsoWarningSolver {
		IsoWarningSolverImpl _impl =
		#if UNITY_EDITOR
			new IsoWarningSolverEditorImpl();
		#else
			new IsoWarningSolverImpl();
		#endif

		public void OnAddIsoObject(IsoObject iso_object) {
			_impl.OnAddIsoObject(iso_object);
		}

		public void OnRemoveIsoObject(IsoObject iso_object) {
			_impl.OnRemoveIsoObject(iso_object);
		}

		public void OnMarkDirtyIsoObject(IsoObject iso_object) {
			_impl.OnMarkDirtyIsoObject(iso_object);
		}

		public void OnDrawGizmos(IsoWorld iso_world) {
			_impl.OnDrawGizmos(iso_world);
		}

		public void StepSortingAction(IsoWorld iso_world) {
			_impl.StepSortingAction(iso_world);
		}

		public void Clear() {
			_impl.Clear();
		}
	}

	class IsoWarningSolverImpl {
		public virtual void OnAddIsoObject(IsoObject iso_object) {}
		public virtual void OnRemoveIsoObject(IsoObject iso_object) {}
		public virtual void OnMarkDirtyIsoObject(IsoObject iso_object) {}
		public virtual void OnDrawGizmos(IsoWorld iso_world) {}
		public virtual void StepSortingAction(IsoWorld iso_world) {}
		public virtual void Clear() {}
	}

	class IsoWarningSolverEditorImpl : IsoWarningSolverImpl {
		const int IsoObjectsPerFrame = 100;

		uint                    _objCounter = 0;
		IsoAssocList<IsoObject> _isoObjects = new IsoAssocList<IsoObject>(47);

		// ---------------------------------------------------------------------
		//
		// Overrides
		//
		// ---------------------------------------------------------------------

		public override void OnAddIsoObject(IsoObject iso_object) {
			_isoObjects.Add(iso_object);
		}

		public override void OnRemoveIsoObject(IsoObject iso_object) {
			_isoObjects.Remove(iso_object);
		}

		public override void StepSortingAction(IsoWorld iso_world) {
			Profiler.BeginSample("IsoWarningSolver.ProcessTransforms");
			ProcessTransforms();
			Profiler.EndSample();
		}

		// ---------------------------------------------------------------------
		//
		// Private
		//
		// ---------------------------------------------------------------------

		void ProcessTransforms() {
			var check_count = Application.isPlaying
				? Mathf.Min(IsoObjectsPerFrame, _isoObjects.Count)
				: _isoObjects.Count;
			for ( var i = 0; i < check_count; ++i ) {
				var obj_index  = (_objCounter++) % _isoObjects.Count;
				var iso_object = _isoObjects[(int)obj_index];
				CheckScaledTransform(iso_object);
				CheckRotatedTransform(iso_object);
				CheckChangedTransform(iso_object);
			}
		}

		void CheckScaledTransform(IsoObject iso_object) {
			if ( iso_object.transform.lossyScale != Vector3.one ) {
				Debug.LogWarning(
					"Don't change 'transform.scale' for IsoObject and its parents!",
					iso_object);
				var trans_iter = iso_object.transform;
				while ( trans_iter ) {
					trans_iter.localScale = Vector3.one;
					trans_iter = trans_iter.parent;
				}
				iso_object.FixTransform();
			}
		}

		void CheckRotatedTransform(IsoObject iso_object) {
			if ( iso_object.transform.rotation != Quaternion.identity ) {
				Debug.LogWarning(
					"Don't change 'transform.rotation' for IsoObject and its parents!",
					iso_object);
				var trans_iter = iso_object.transform;
				while ( trans_iter ) {
					trans_iter.localRotation = Quaternion.identity;
					trans_iter = trans_iter.parent;
				}
				iso_object.FixTransform();
			}
		}

		void CheckChangedTransform(IsoObject iso_object) {
			var iso_world = iso_object.isoWorld;
			if ( iso_world ) {
				var precision        = Mathf.Min(iso_world.tileSize, iso_world.tileHeight) * 0.01f;
				var needed_position  = iso_world.IsoToScreen(iso_object.position);
				var current_position = iso_object.transform.position;
				if ( !IsoUtils.Vec2Approximately(needed_position, current_position, precision) ) {
					Debug.LogWarning(
						"Don't change 'IsoObject.transform.position' manually!\n" +
						"Use 'IsoObject.position' instead.",
						iso_object);
					iso_object.FixTransform();
				}
			}
		}
	}
}
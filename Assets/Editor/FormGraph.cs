using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UIElements;

public class FormGraph : EditorWindow
{
   private FormPlannerGraphView _graphView;
   private string _filename="New Form Graph";
   
   [MenuItem("Template/Form Planner")]
   public static void OpenFormGraphWindow()
   {
      FormGraph window = GetWindow<FormGraph>();
      window.titleContent = new GUIContent("Form Planner");
   }

   private void OnEnable()
   {
      ConstructGraphView();
      GenerateToolbar();
   }

   private void ConstructGraphView()
   {
      _graphView = new FormPlannerGraphView(this)
      {
         name = "Form Planner"
      };
      
      _graphView.StretchToParentSize();
      rootVisualElement.Add(_graphView);
   }

   private void GenerateToolbar()
   {
      var toolbar = new Toolbar();
      //
      var fileNameTextField = new TextField("Filename:");
      fileNameTextField.SetValueWithoutNotify("New Form Graph");
      fileNameTextField.MarkDirtyRepaint();
      fileNameTextField.RegisterValueChangedCallback(evt => _filename = evt.newValue);
      toolbar.Add(fileNameTextField);
      //
      toolbar.Add(new Button(()=>RequestDataOperation(true)){text = "Save"});
      toolbar.Add(new Button(()=>RequestDataOperation(false)){text = "Load"});
      //
      rootVisualElement.Add(toolbar);
   }

   private void RequestDataOperation(bool save)
   {
      if (string.IsNullOrEmpty(_filename))
      {
         EditorUtility.DisplayDialog("Invalid file name", "Please enter a valid file name", "OK");
         return;
      }
      //
      var saveUtility = GraphSaveUtility.GetInstance(_graphView);
      if(save)
         saveUtility.SaveGraph(_filename);
      else
         saveUtility.LoadGraph(_filename);
   }

   private void OnDisable()
   {
      rootVisualElement.Remove(_graphView);
   }
}

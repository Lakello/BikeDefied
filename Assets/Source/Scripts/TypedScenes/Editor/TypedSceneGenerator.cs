#if UNITY_EDITOR
using BikeDefied.FSM.Game;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;

namespace BikeDefied.TypedScenes.Editor
{
    public static class TypedSceneGenerator
    {
        public static string Generate(AnalyzableScene scene)
        {
            var sceneName = scene.Name;
            var targetUnit = new CodeCompileUnit();
            var targetNamespace = new CodeNamespace(TypedSceneSettings.Namespace);
            var targetClass = new CodeTypeDeclaration(sceneName);
            targetNamespace.Imports.Add(new CodeNamespaceImport("UnityEngine.SceneManagement"));
            targetNamespace.Imports.Add(new CodeNamespaceImport("BikeDefied.FSM.Game"));
            targetNamespace.Imports.Add(new CodeNamespaceImport("BikeDefied.FSM"));
            targetClass.BaseTypes.Add(new CodeTypeReference(
                "TypedScene",
                new CodeTypeReference[] { new CodeTypeReference(typeof(GameStateMachine)) }));

            targetClass.TypeAttributes = System.Reflection.TypeAttributes.Class | System.Reflection.TypeAttributes.Public;

            AddConstantValue(targetClass, typeof(string), "_sceneName", sceneName);

            var loadingParameters = SceneAnalyzer.GetLoadingParameters(scene);
            foreach (var loadingParameter in loadingParameters)
            {
                AddLoadingMethod(targetClass, loadingParameter);
                AddLoadingMethod(targetClass, loadingParameter, asyncLoad: true);
                AddLoadingMethod(targetClass, loadingParameter, isStateLoad: true, machine: typeof(GameStateMachine));
            }

            targetNamespace.Types.Add(targetClass);
            targetUnit.Namespaces.Add(targetNamespace);

            var provider = CodeDomProvider.CreateProvider("CSharp");
            var options = new CodeGeneratorOptions();
            options.BracingStyle = "C";

            var code = new StringWriter();
            provider.GenerateCodeFromCompileUnit(targetUnit, code, options);

            return code.ToString();
        }

        private static void AddConstantValue(CodeTypeDeclaration targetClass, Type type, string name, string value)
        {
            var pathConstant = new CodeMemberField(type, name);
            pathConstant.Attributes = MemberAttributes.Private | MemberAttributes.Const;
            pathConstant.InitExpression = new CodePrimitiveExpression(value);
            targetClass.Members.Add(pathConstant);
        }

        private static void AddLoadingMethod(
            CodeTypeDeclaration targetClass,
            Type parameterType = null, 
            bool asyncLoad = false,
            bool isStateLoad = false,
            Type machine = null)
        {
            CodeMemberMethod loadMethod = new CodeMemberMethod
            {
                Name = asyncLoad ? "LoadAsync" : "Load",
                Attributes = MemberAttributes.Public | MemberAttributes.Static
            };

            string loadingStatement = "LoadScene";

            if (isStateLoad)
            {
                loadingStatement += "<TState>";
            }

            loadingStatement += "(_sceneName, loadSceneMode";

            if (isStateLoad)
            {
                if (machine != null)
                {
                    AddParameter(machine, nameof(machine));
                }

                var targetTypeParameter = new CodeTypeParameter("TState");

                var tstate = new CodeTypeReference("State");
                tstate.TypeArguments.Add(new CodeTypeReference("GameStateMachine"));

                targetTypeParameter.Constraints.Add(tstate);

                loadMethod.TypeParameters.Add(targetTypeParameter);
            }

            if (parameterType != null)
            {
                AddParameter(parameterType, "argument");
            }

            if (asyncLoad)
            {
                loadMethod.ReturnType = new CodeTypeReference(typeof(AsyncOperation));
                loadingStatement = "return " + loadingStatement;
            }

            loadingStatement += ")";

            CodeParameterDeclarationExpression loadingModeParameter = new CodeParameterDeclarationExpression(
                nameof(LoadSceneMode), "loadSceneMode = LoadSceneMode.Single");
            loadMethod.Parameters.Add(loadingModeParameter);
            loadMethod.Statements.Add(new CodeSnippetExpression(loadingStatement));
            targetClass.Members.Add(loadMethod);
            return;

            void AddParameter(Type type, string argumentName)
            {
                var parameter = new CodeParameterDeclarationExpression(type, argumentName);
                loadMethod.Parameters.Add(parameter);
                loadingStatement += $", {argumentName}";
            }
        }
    }
}
#endif
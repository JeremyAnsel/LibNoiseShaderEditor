using DynamicData;
using JeremyAnsel.LibNoiseShader.IO;
using JeremyAnsel.LibNoiseShader.IO.FileBuilders;
using JeremyAnsel.LibNoiseShader.IO.FileModules;
using JeremyAnsel.LibNoiseShader.IO.FileRenderers;
using LibNoiseShaderEditor.ViewModels;
using LibNoiseShaderEditor.ViewModels.Builders;
using LibNoiseShaderEditor.ViewModels.Modules;
using LibNoiseShaderEditor.ViewModels.Renderers;
using NodeNetwork.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace LibNoiseShaderEditor.Models
{
    public sealed class EditorLoadContext
    {
        private readonly Dictionary<IFileModule, ModuleNodeViewModel> _modules = new();

        private readonly Dictionary<IFileBuilder, BuilderNodeViewModel> _builders = new();

        private readonly Dictionary<IFileRenderer, RendererNodeViewModel> _renderers = new();

        private ModuleNodeViewModel AddModule(IFileModule moduleFile, ModuleNodeViewModel moduleViewModel)
        {
            if (!_modules.ContainsKey(moduleFile))
            {
                _modules.Add(moduleFile, moduleViewModel);
            }

            return _modules[moduleFile];
        }

        private ModuleNodeViewModel GetModule(IFileModule moduleFile)
        {
            if (moduleFile is null)
            {
                return null;
            }

            if (!_modules.TryGetValue(moduleFile, out ModuleNodeViewModel module))
            {
                return null;
            }

            return module;
        }

        private BuilderNodeViewModel AddBuilder(IFileBuilder builderFile, BuilderNodeViewModel builderViewModel)
        {
            if (!_builders.ContainsKey(builderFile))
            {
                _builders.Add(builderFile, builderViewModel);
            }

            return _builders[builderFile];
        }

        private BuilderNodeViewModel GetBuilder(IFileBuilder builderFile)
        {
            if (builderFile is null)
            {
                return null;
            }

            if (!_builders.TryGetValue(builderFile, out BuilderNodeViewModel builder))
            {
                return null;
            }

            return builder;
        }

        private RendererNodeViewModel AddRenderer(IFileRenderer rendererFile, RendererNodeViewModel rendererViewModel)
        {
            if (!_renderers.ContainsKey(rendererFile))
            {
                _renderers.Add(rendererFile, rendererViewModel);
            }

            return _renderers[rendererFile];
        }

        private RendererNodeViewModel GetRenderer(IFileRenderer rendererFile)
        {
            if (rendererFile is null)
            {
                return null;
            }

            if (!_renderers.TryGetValue(rendererFile, out RendererNodeViewModel renderer))
            {
                return null;
            }

            return renderer;
        }

        private static void CreateConnection(MainViewModel mainViewModel, NodeInputViewModel input, NodeViewModel output)
        {
            if (output is null)
            {
                return;
            }

            mainViewModel.NetworkViewModel.Connections.Add(
                new ConnectionViewModel(
                    mainViewModel.NetworkViewModel,
                    input,
                    output.Outputs.Items.First()));
        }

        public static void ImportLibNoiseShaderFile(LibNoiseShaderFile file, MainViewModel mainViewModel)
        {
            if (file is null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            if (mainViewModel is null)
            {
                throw new ArgumentNullException(nameof(mainViewModel));
            }

            using var dispose = mainViewModel.DelayChangeNotifications();
            var context = new EditorLoadContext();

            LoadModules(file, mainViewModel, context);
            LoadBuilders(file, mainViewModel, context);
            LoadRenderers(file, mainViewModel, context);
        }

        public static void LoadLibNoiseShaderFile(LibNoiseShaderFile file, MainViewModel mainViewModel)
        {
            if (file is null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            if (mainViewModel is null)
            {
                throw new ArgumentNullException(nameof(mainViewModel));
            }

            using var dispose = mainViewModel.DelayChangeNotifications();
            var context = new EditorLoadContext();

            LoadModules(file, mainViewModel, context);
            LoadBuilders(file, mainViewModel, context);
            LoadRenderers(file, mainViewModel, context);
        }

        private static void LoadModules(LibNoiseShaderFile file, MainViewModel mainViewModel, EditorLoadContext context)
        {
            foreach (IFileModule fileModule in file.Modules)
            {
                _ = fileModule switch
                {
                    AbsFileModule m => BuildAbsModuleNode(mainViewModel, context, m),
                    AddFileModule m => BuildAddModuleNode(mainViewModel, context, m),
                    BillowFileModule m => BuildBillowModuleNode(mainViewModel, context, m),
                    BlendFileModule m => BuildBlendModuleNode(mainViewModel, context, m),
                    CacheFileModule m => BuildCacheModuleNode(mainViewModel, context, m),
                    CheckerboardFileModule m => BuildCheckerboardModuleNode(mainViewModel, context, m),
                    ClampFileModule m => BuildClampModuleNode(mainViewModel, context, m),
                    ConstantFileModule m => BuildConstantModuleNode(mainViewModel, context, m),
                    CurveFileModule m => BuildCurveModuleNode(mainViewModel, context, m),
                    CylinderFileModule m => BuildCylinderModuleNode(mainViewModel, context, m),
                    DisplaceFileModule m => BuildDisplaceModuleNode(mainViewModel, context, m),
                    ExponentFileModule m => BuildExponentModuleNode(mainViewModel, context, m),
                    InvertFileModule m => BuildInvertModuleNode(mainViewModel, context, m),
                    LineFileModule m => BuildLineModuleNode(mainViewModel, context, m),
                    MaxFileModule m => BuildMaxModuleNode(mainViewModel, context, m),
                    MinFileModule m => BuildMinModuleNode(mainViewModel, context, m),
                    MultiplyFileModule m => BuildMultiplyModuleNode(mainViewModel, context, m),
                    PerlinFileModule m => BuildPerlinModuleNode(mainViewModel, context, m),
                    PowerFileModule m => BuildPowerModuleNode(mainViewModel, context, m),
                    RidgedMultiFileModule m => BuildRidgedMultiModuleNode(mainViewModel, context, m),
                    RotatePointFileModule m => BuildRotatePointModuleNode(mainViewModel, context, m),
                    ScaleBiasFileModule m => BuildScaleBiasModuleNode(mainViewModel, context, m),
                    ScalePointFileModule m => BuildScalePointModuleNode(mainViewModel, context, m),
                    SelectorFileModule m => BuildSelectorModuleNode(mainViewModel, context, m),
                    SphereFileModule m => BuildSphereModuleNode(mainViewModel, context, m),
                    TerraceFileModule m => BuildTerraceModuleNode(mainViewModel, context, m),
                    TranslatePointFileModule m => BuildTranslatePointModuleNode(mainViewModel, context, m),
                    TurbulenceFileModule m => BuildTurbulencePointModuleNode(mainViewModel, context, m),
                    VoronoiFileModule m => BuildVoronoiModuleNode(mainViewModel, context, m),
                    _ => throw new NotSupportedException($"Not supported module type found: {fileModule.GetType().Name}"),
                };
            }
        }

        private static void LoadBuilders(LibNoiseShaderFile file, MainViewModel mainViewModel, EditorLoadContext context)
        {
            foreach (IFileBuilder fileBuilder in file.Builders)
            {
                _ = fileBuilder switch
                {
                    CylinderFileBuilder m => BuildCylinderBuilderNode(mainViewModel, context, m),
                    PlaneFileBuilder m => BuildPlaneBuilderNode(mainViewModel, context, m),
                    SphereFileBuilder m => BuildSphereBuilderNode(mainViewModel, context, m),
                    _ => throw new NotSupportedException($"Not supported builder type found: {fileBuilder.GetType().Name}"),
                };
            }
        }

        private static void LoadRenderers(LibNoiseShaderFile file, MainViewModel mainViewModel, EditorLoadContext context)
        {
            foreach (IFileRenderer fileRenderer in file.Renderers)
            {
                _ = fileRenderer switch
                {
                    BlendFileRenderer m => BuildBlendRendererNode(mainViewModel, context, m),
                    ImageFileRenderer m => BuildImageRendererNode(mainViewModel, context, m),
                    NormalFileRenderer m => BuildNormalRendererNode(mainViewModel, context, m),
                    _ => throw new NotSupportedException($"Not supported renderer type found: {fileRenderer.GetType().Name}"),
                };
            }
        }

        private static void BuildModuleNode(MainViewModel mainViewModel, EditorLoadContext context, ModuleNodeViewModel vm, IFileModule m)
        {
            vm.Position = new System.Windows.Point(m.PositionX, m.PositionY);

            context.AddModule(m, vm);
            mainViewModel.NetworkViewModel.Nodes.Add(vm);
        }

        private static ModuleNodeViewModel BuildAbsModuleNode(MainViewModel mainViewModel, EditorLoadContext context, AbsFileModule m)
        {
            var vm = new AbsModuleViewModel(mainViewModel);
            vm.Setting.Name = m.Name;

            BuildModuleNode(mainViewModel, context, vm, m);
            CreateConnection(mainViewModel, vm.Input1, context.GetModule(m.Input1));

            return vm;
        }

        private static ModuleNodeViewModel BuildAddModuleNode(MainViewModel mainViewModel, EditorLoadContext context, AddFileModule m)
        {
            var vm = new AddModuleViewModel(mainViewModel);
            vm.Setting.Name = m.Name;

            BuildModuleNode(mainViewModel, context, vm, m);
            CreateConnection(mainViewModel, vm.Input1, context.GetModule(m.Input1));
            CreateConnection(mainViewModel, vm.Input2, context.GetModule(m.Input2));

            return vm;
        }

        private static ModuleNodeViewModel BuildBillowModuleNode(MainViewModel mainViewModel, EditorLoadContext context, BillowFileModule m)
        {
            var vm = new BillowModuleViewModel(mainViewModel);
            vm.Setting.Name = m.Name;
            vm.Setting.Frequency = m.Frequency;
            vm.Setting.Lacunarity = m.Lacunarity;
            vm.Setting.OctaveCount = m.OctaveCount;
            vm.Setting.Persistence = m.Persistence;
            vm.Setting.SeedOffset = m.SeedOffset;

            BuildModuleNode(mainViewModel, context, vm, m);

            return vm;
        }

        private static ModuleNodeViewModel BuildBlendModuleNode(MainViewModel mainViewModel, EditorLoadContext context, BlendFileModule m)
        {
            var vm = new BlendModuleViewModel(mainViewModel);
            vm.Setting.Name = m.Name;

            BuildModuleNode(mainViewModel, context, vm, m);
            CreateConnection(mainViewModel, vm.Input1, context.GetModule(m.Input1));
            CreateConnection(mainViewModel, vm.Input2, context.GetModule(m.Input2));
            CreateConnection(mainViewModel, vm.Control, context.GetModule(m.Control));

            return vm;
        }

        private static ModuleNodeViewModel BuildCacheModuleNode(MainViewModel mainViewModel, EditorLoadContext context, CacheFileModule m)
        {
            var vm = new CacheModuleViewModel(mainViewModel);
            vm.Setting.Name = m.Name;

            BuildModuleNode(mainViewModel, context, vm, m);
            CreateConnection(mainViewModel, vm.Input1, context.GetModule(m.Input1));

            return vm;
        }

        private static ModuleNodeViewModel BuildCheckerboardModuleNode(MainViewModel mainViewModel, EditorLoadContext context, CheckerboardFileModule m)
        {
            var vm = new CheckerboardModuleViewModel(mainViewModel);
            vm.Setting.Name = m.Name;

            BuildModuleNode(mainViewModel, context, vm, m);

            return vm;
        }

        private static ModuleNodeViewModel BuildClampModuleNode(MainViewModel mainViewModel, EditorLoadContext context, ClampFileModule m)
        {
            var vm = new ClampModuleViewModel(mainViewModel);
            vm.Setting.Name = m.Name;
            vm.Setting.LowerBound = m.LowerBound;
            vm.Setting.UpperBound = m.UpperBound;

            BuildModuleNode(mainViewModel, context, vm, m);
            CreateConnection(mainViewModel, vm.Input1, context.GetModule(m.Input1));

            return vm;
        }

        private static ModuleNodeViewModel BuildConstantModuleNode(MainViewModel mainViewModel, EditorLoadContext context, ConstantFileModule m)
        {
            var vm = new ConstantModuleViewModel(mainViewModel);
            vm.Setting.Name = m.Name;
            vm.Setting.ConstantValue = m.ConstantValue;

            BuildModuleNode(mainViewModel, context, vm, m);

            return vm;
        }

        private static ModuleNodeViewModel BuildCurveModuleNode(MainViewModel mainViewModel, EditorLoadContext context, CurveFileModule m)
        {
            var vm = new CurveModuleViewModel(mainViewModel);
            vm.Setting.Name = m.Name;
            vm.Setting.ControlPoints = new Dictionary<float, float>(m.ControlPoints);

            BuildModuleNode(mainViewModel, context, vm, m);
            CreateConnection(mainViewModel, vm.Input1, context.GetModule(m.Input1));

            return vm;
        }

        private static ModuleNodeViewModel BuildCylinderModuleNode(MainViewModel mainViewModel, EditorLoadContext context, CylinderFileModule m)
        {
            var vm = new CylinderModuleViewModel(mainViewModel);
            vm.Setting.Name = m.Name;
            vm.Setting.Frequency = m.Frequency;

            BuildModuleNode(mainViewModel, context, vm, m);

            return vm;
        }

        private static ModuleNodeViewModel BuildDisplaceModuleNode(MainViewModel mainViewModel, EditorLoadContext context, DisplaceFileModule m)
        {
            var vm = new DisplaceModuleViewModel(mainViewModel);
            vm.Setting.Name = m.Name;

            BuildModuleNode(mainViewModel, context, vm, m);
            CreateConnection(mainViewModel, vm.Input1, context.GetModule(m.Input1));
            CreateConnection(mainViewModel, vm.DisplaceX, context.GetModule(m.DisplaceX));
            CreateConnection(mainViewModel, vm.DisplaceY, context.GetModule(m.DisplaceY));
            CreateConnection(mainViewModel, vm.DisplaceZ, context.GetModule(m.DisplaceZ));

            return vm;
        }

        private static ModuleNodeViewModel BuildExponentModuleNode(MainViewModel mainViewModel, EditorLoadContext context, ExponentFileModule m)
        {
            var vm = new ExponentModuleViewModel(mainViewModel);
            vm.Setting.Name = m.Name;
            vm.Setting.ExponentValue = m.ExponentValue;

            BuildModuleNode(mainViewModel, context, vm, m);
            CreateConnection(mainViewModel, vm.Input1, context.GetModule(m.Input1));

            return vm;
        }

        private static ModuleNodeViewModel BuildInvertModuleNode(MainViewModel mainViewModel, EditorLoadContext context, InvertFileModule m)
        {
            var vm = new InvertModuleViewModel(mainViewModel);
            vm.Setting.Name = m.Name;

            BuildModuleNode(mainViewModel, context, vm, m);
            CreateConnection(mainViewModel, vm.Input1, context.GetModule(m.Input1));

            return vm;
        }

        private static ModuleNodeViewModel BuildLineModuleNode(MainViewModel mainViewModel, EditorLoadContext context, LineFileModule m)
        {
            var vm = new LineModuleViewModel(mainViewModel);
            vm.Setting.Name = m.Name;
            vm.Setting.Attenuate = m.Attenuate;
            vm.Setting.StartPointX = m.StartPointX;
            vm.Setting.StartPointY = m.StartPointY;
            vm.Setting.StartPointZ = m.StartPointZ;
            vm.Setting.EndPointX = m.EndPointX;
            vm.Setting.EndPointY = m.EndPointY;
            vm.Setting.EndPointZ = m.EndPointZ;

            BuildModuleNode(mainViewModel, context, vm, m);
            CreateConnection(mainViewModel, vm.Input1, context.GetModule(m.Input1));

            return vm;
        }

        private static ModuleNodeViewModel BuildMaxModuleNode(MainViewModel mainViewModel, EditorLoadContext context, MaxFileModule m)
        {
            var vm = new MaxModuleViewModel(mainViewModel);
            vm.Setting.Name = m.Name;

            BuildModuleNode(mainViewModel, context, vm, m);
            CreateConnection(mainViewModel, vm.Input1, context.GetModule(m.Input1));
            CreateConnection(mainViewModel, vm.Input2, context.GetModule(m.Input2));

            return vm;
        }

        private static ModuleNodeViewModel BuildMinModuleNode(MainViewModel mainViewModel, EditorLoadContext context, MinFileModule m)
        {
            var vm = new MinModuleViewModel(mainViewModel);
            vm.Setting.Name = m.Name;

            BuildModuleNode(mainViewModel, context, vm, m);
            CreateConnection(mainViewModel, vm.Input1, context.GetModule(m.Input1));
            CreateConnection(mainViewModel, vm.Input2, context.GetModule(m.Input2));

            return vm;
        }

        private static ModuleNodeViewModel BuildMultiplyModuleNode(MainViewModel mainViewModel, EditorLoadContext context, MultiplyFileModule m)
        {
            var vm = new MultiplyModuleViewModel(mainViewModel);
            vm.Setting.Name = m.Name;

            BuildModuleNode(mainViewModel, context, vm, m);
            CreateConnection(mainViewModel, vm.Input1, context.GetModule(m.Input1));
            CreateConnection(mainViewModel, vm.Input2, context.GetModule(m.Input2));

            return vm;
        }

        private static ModuleNodeViewModel BuildPerlinModuleNode(MainViewModel mainViewModel, EditorLoadContext context, PerlinFileModule m)
        {
            var vm = new PerlinModuleViewModel(mainViewModel);
            vm.Setting.Name = m.Name;
            vm.Setting.Frequency = m.Frequency;
            vm.Setting.Lacunarity = m.Lacunarity;
            vm.Setting.OctaveCount = m.OctaveCount;
            vm.Setting.Persistence = m.Persistence;
            vm.Setting.SeedOffset = m.SeedOffset;

            BuildModuleNode(mainViewModel, context, vm, m);

            return vm;
        }

        private static ModuleNodeViewModel BuildPowerModuleNode(MainViewModel mainViewModel, EditorLoadContext context, PowerFileModule m)
        {
            var vm = new PowerModuleViewModel(mainViewModel);
            vm.Setting.Name = m.Name;

            BuildModuleNode(mainViewModel, context, vm, m);
            CreateConnection(mainViewModel, vm.Input1, context.GetModule(m.Input1));
            CreateConnection(mainViewModel, vm.Input2, context.GetModule(m.Input2));

            return vm;
        }

        private static ModuleNodeViewModel BuildRidgedMultiModuleNode(MainViewModel mainViewModel, EditorLoadContext context, RidgedMultiFileModule m)
        {
            var vm = new RidgedMultiModuleViewModel(mainViewModel);
            vm.Setting.Name = m.Name;
            vm.Setting.Frequency = m.Frequency;
            vm.Setting.Lacunarity = m.Lacunarity;
            vm.Setting.Offset = m.Offset;
            vm.Setting.Gain = m.Gain;
            vm.Setting.Exponent = m.Exponent;
            vm.Setting.OctaveCount = m.OctaveCount;
            vm.Setting.SeedOffset = m.SeedOffset;

            BuildModuleNode(mainViewModel, context, vm, m);

            return vm;
        }

        private static ModuleNodeViewModel BuildRotatePointModuleNode(MainViewModel mainViewModel, EditorLoadContext context, RotatePointFileModule m)
        {
            var vm = new RotatePointModuleViewModel(mainViewModel);
            vm.Setting.Name = m.Name;
            vm.Setting.AngleX = m.AngleX;
            vm.Setting.AngleY = m.AngleY;
            vm.Setting.AngleZ = m.AngleZ;

            BuildModuleNode(mainViewModel, context, vm, m);
            CreateConnection(mainViewModel, vm.Input1, context.GetModule(m.Input1));

            return vm;
        }

        private static ModuleNodeViewModel BuildScaleBiasModuleNode(MainViewModel mainViewModel, EditorLoadContext context, ScaleBiasFileModule m)
        {
            var vm = new ScaleBiasModuleViewModel(mainViewModel);
            vm.Setting.Name = m.Name;
            vm.Setting.Bias = m.Bias;
            vm.Setting.Scale = m.Scale;

            BuildModuleNode(mainViewModel, context, vm, m);
            CreateConnection(mainViewModel, vm.Input1, context.GetModule(m.Input1));

            return vm;
        }

        private static ModuleNodeViewModel BuildScalePointModuleNode(MainViewModel mainViewModel, EditorLoadContext context, ScalePointFileModule m)
        {
            var vm = new ScalePointModuleViewModel(mainViewModel);
            vm.Setting.Name = m.Name;
            vm.Setting.ScaleX = m.ScaleX;
            vm.Setting.ScaleY = m.ScaleY;
            vm.Setting.ScaleZ = m.ScaleZ;

            BuildModuleNode(mainViewModel, context, vm, m);
            CreateConnection(mainViewModel, vm.Input1, context.GetModule(m.Input1));

            return vm;
        }

        private static ModuleNodeViewModel BuildSelectorModuleNode(MainViewModel mainViewModel, EditorLoadContext context, SelectorFileModule m)
        {
            var vm = new SelectorModuleViewModel(mainViewModel);
            vm.Setting.Name = m.Name;
            vm.Setting.EdgeFalloff = m.EdgeFalloff;
            vm.Setting.LowerBound = m.LowerBound;
            vm.Setting.UpperBound = m.UpperBound;

            BuildModuleNode(mainViewModel, context, vm, m);
            CreateConnection(mainViewModel, vm.Input1, context.GetModule(m.Input1));
            CreateConnection(mainViewModel, vm.Input2, context.GetModule(m.Input2));
            CreateConnection(mainViewModel, vm.Control, context.GetModule(m.Control));

            return vm;
        }

        private static ModuleNodeViewModel BuildSphereModuleNode(MainViewModel mainViewModel, EditorLoadContext context, SphereFileModule m)
        {
            var vm = new SphereModuleViewModel(mainViewModel);
            vm.Setting.Name = m.Name;
            vm.Setting.Frequency = m.Frequency;

            BuildModuleNode(mainViewModel, context, vm, m);

            return vm;
        }

        private static ModuleNodeViewModel BuildTerraceModuleNode(MainViewModel mainViewModel, EditorLoadContext context, TerraceFileModule m)
        {
            var vm = new TerraceModuleViewModel(mainViewModel);
            vm.Setting.Name = m.Name;
            vm.Setting.ControlPoints = new List<float>(m.ControlPoints);
            vm.Setting.IsInverted = m.IsInverted;

            BuildModuleNode(mainViewModel, context, vm, m);
            CreateConnection(mainViewModel, vm.Input1, context.GetModule(m.Input1));

            return vm;
        }

        private static ModuleNodeViewModel BuildTranslatePointModuleNode(MainViewModel mainViewModel, EditorLoadContext context, TranslatePointFileModule m)
        {
            var vm = new TranslatePointModuleViewModel(mainViewModel);
            vm.Setting.Name = m.Name;
            vm.Setting.TranslateX = m.TranslateX;
            vm.Setting.TranslateY = m.TranslateY;
            vm.Setting.TranslateZ = m.TranslateZ;

            BuildModuleNode(mainViewModel, context, vm, m);
            CreateConnection(mainViewModel, vm.Input1, context.GetModule(m.Input1));

            return vm;
        }

        private static ModuleNodeViewModel BuildTurbulencePointModuleNode(MainViewModel mainViewModel, EditorLoadContext context, TurbulenceFileModule m)
        {
            var vm = new TurbulenceModuleViewModel(mainViewModel);
            vm.Setting.Name = m.Name;
            vm.Setting.Frequency = m.Frequency;
            vm.Setting.Power = m.Power;
            vm.Setting.Roughness = m.Roughness;
            vm.Setting.SeedOffset = m.SeedOffset;

            BuildModuleNode(mainViewModel, context, vm, m);
            CreateConnection(mainViewModel, vm.Input1, context.GetModule(m.Input1));

            return vm;
        }

        private static ModuleNodeViewModel BuildVoronoiModuleNode(MainViewModel mainViewModel, EditorLoadContext context, VoronoiFileModule m)
        {
            var vm = new VoronoiModuleViewModel(mainViewModel);
            vm.Setting.Name = m.Name;
            vm.Setting.IsDistanceApplied = m.IsDistanceApplied;
            vm.Setting.Displacement = m.Displacement;
            vm.Setting.Frequency = m.Frequency;
            vm.Setting.SeedOffset = m.SeedOffset;

            BuildModuleNode(mainViewModel, context, vm, m);

            return vm;
        }

        private static void BuildBuilderNode(MainViewModel mainViewModel, EditorLoadContext context, BuilderNodeViewModel vm, IFileBuilder m)
        {
            vm.Position = new System.Windows.Point(m.PositionX, m.PositionY);

            context.AddBuilder(m, vm);
            mainViewModel.NetworkViewModel.Nodes.Add(vm);
        }

        private static BuilderNodeViewModel BuildCylinderBuilderNode(MainViewModel mainViewModel, EditorLoadContext context, CylinderFileBuilder m)
        {
            var vm = new CylinderBuilderViewModel(mainViewModel);
            vm.Setting.Name = m.Name;
            vm.Setting.LowerAngleBound = m.LowerAngleBound;
            vm.Setting.LowerHeightBound = m.LowerHeightBound;
            vm.Setting.UpperAngleBound = m.UpperAngleBound;
            vm.Setting.UpperHeightBound = m.UpperHeightBound;

            BuildBuilderNode(mainViewModel, context, vm, m);
            CreateConnection(mainViewModel, vm.Source, context.GetModule(m.Source));

            return vm;
        }

        private static BuilderNodeViewModel BuildPlaneBuilderNode(MainViewModel mainViewModel, EditorLoadContext context, PlaneFileBuilder m)
        {
            var vm = new PlaneBuilderViewModel(mainViewModel);
            vm.Setting.Name = m.Name;
            vm.Setting.IsSeamless = m.IsSeamless;
            vm.Setting.LowerBoundX = m.LowerBoundX;
            vm.Setting.UpperBoundX = m.UpperBoundX;
            vm.Setting.LowerBoundY = m.LowerBoundY;
            vm.Setting.UpperBoundY = m.UpperBoundY;

            BuildBuilderNode(mainViewModel, context, vm, m);
            CreateConnection(mainViewModel, vm.Source, context.GetModule(m.Source));

            return vm;
        }

        private static BuilderNodeViewModel BuildSphereBuilderNode(MainViewModel mainViewModel, EditorLoadContext context, SphereFileBuilder m)
        {
            var vm = new SphereBuilderViewModel(mainViewModel);
            vm.Setting.Name = m.Name;
            vm.Setting.SouthLatBound = m.SouthLatBound;
            vm.Setting.NorthLatBound = m.NorthLatBound;
            vm.Setting.WestLonBound = m.WestLonBound;
            vm.Setting.EastLonBound = m.EastLonBound;

            BuildBuilderNode(mainViewModel, context, vm, m);
            CreateConnection(mainViewModel, vm.Source, context.GetModule(m.Source));

            return vm;
        }

        private static void BuildRendererNode(MainViewModel mainViewModel, EditorLoadContext context, RendererNodeViewModel vm, IFileRenderer m)
        {
            vm.Position = new System.Windows.Point(m.PositionX, m.PositionY);

            context.AddRenderer(m, vm);
            mainViewModel.NetworkViewModel.Nodes.Add(vm);
        }

        private static RendererNodeViewModel BuildBlendRendererNode(MainViewModel mainViewModel, EditorLoadContext context, BlendFileRenderer m)
        {
            var vm = new BlendRendererViewModel(mainViewModel);
            vm.Setting.Name = m.Name;

            BuildRendererNode(mainViewModel, context, vm, m);
            CreateConnection(mainViewModel, vm.Input1, context.GetRenderer(m.Input1));
            CreateConnection(mainViewModel, vm.Input2, context.GetRenderer(m.Input2));

            return vm;
        }

        private static RendererNodeViewModel BuildImageRendererNode(MainViewModel mainViewModel, EditorLoadContext context, ImageFileRenderer m)
        {
            var vm = new ImageRendererViewModel(mainViewModel);
            vm.Setting.Name = m.Name;
            vm.Setting.IsWrapEnabled = m.IsWrapEnabled;
            vm.Setting.IsLightEnabled = m.IsLightEnabled;
            vm.Setting.LightAzimuth = m.LightAzimuth;
            vm.Setting.LightBrightness = m.LightBrightness;
            vm.Setting.LightColor = Color.FromArgb(
                m.LightColor.A,
                m.LightColor.R,
                m.LightColor.G,
                m.LightColor.B);
            vm.Setting.LightContrast = m.LightContrast;
            vm.Setting.LightElevation = m.LightElevation;
            vm.Setting.LightIntensity = m.LightIntensity;
            vm.Setting.GradientPoints = m.GradientPoints.ToDictionary(
                t => t.Key,
                t => Color.FromArgb(t.Value.A, t.Value.R, t.Value.G, t.Value.B));

            BuildRendererNode(mainViewModel, context, vm, m);
            CreateConnection(mainViewModel, vm.Source, context.GetBuilder(m.Source));

            return vm;
        }

        private static RendererNodeViewModel BuildNormalRendererNode(MainViewModel mainViewModel, EditorLoadContext context, NormalFileRenderer m)
        {
            var vm = new NormalRendererViewModel(mainViewModel);
            vm.Setting.Name = m.Name;

            BuildRendererNode(mainViewModel, context, vm, m);
            CreateConnection(mainViewModel, vm.Source, context.GetBuilder(m.Source));

            return vm;
        }
    }
}

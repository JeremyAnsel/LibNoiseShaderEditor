using JeremyAnsel.LibNoiseShader;
using JeremyAnsel.LibNoiseShader.Builders;
using JeremyAnsel.LibNoiseShader.IO;
using JeremyAnsel.LibNoiseShader.IO.FileBuilders;
using JeremyAnsel.LibNoiseShader.IO.FileModules;
using JeremyAnsel.LibNoiseShader.IO.FileRenderers;
using JeremyAnsel.LibNoiseShader.Modules;
using JeremyAnsel.LibNoiseShader.Renderers;
using LibNoiseShaderEditor.ViewModels.Builders;
using LibNoiseShaderEditor.ViewModels.Modules;
using LibNoiseShaderEditor.ViewModels.Renderers;
using NodeNetwork.Toolkit.ValueNode;
using NodeNetwork.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace LibNoiseShaderEditor.Models
{
    public sealed class EditorWriteContext
    {
        private readonly Dictionary<ModuleNodeViewModel, IFileModule> _modules = new();

        private readonly Dictionary<BuilderNodeViewModel, IFileBuilder> _builders = new();

        private readonly Dictionary<RendererNodeViewModel, IFileRenderer> _renderers = new();

        private IFileModule AddModule(ModuleNodeViewModel moduleViewModel, IFileModule moduleFile)
        {
            if (!_modules.ContainsKey(moduleViewModel))
            {
                _modules.Add(moduleViewModel, moduleFile);
            }

            return _modules[moduleViewModel];
        }

        private IFileModule GetModule(ModuleNodeViewModel moduleViewModel)
        {
            if (moduleViewModel is null)
            {
                return null;
            }

            if (!_modules.TryGetValue(moduleViewModel, out IFileModule module))
            {
                return null;
            }

            return module;
        }

        private IFileBuilder AddBuilder(BuilderNodeViewModel builderViewModel, IFileBuilder builderFile)
        {
            if (!_builders.ContainsKey(builderViewModel))
            {
                _builders.Add(builderViewModel, builderFile);
            }

            return _builders[builderViewModel];
        }

        private IFileBuilder GetBuilder(BuilderNodeViewModel builderViewModel)
        {
            if (builderViewModel is null)
            {
                return null;
            }

            if (!_builders.TryGetValue(builderViewModel, out IFileBuilder builder))
            {
                return null;
            }

            return builder;
        }

        private IFileRenderer AddRenderer(RendererNodeViewModel rendererViewModel, IFileRenderer rendererFile)
        {
            if (!_renderers.ContainsKey(rendererViewModel))
            {
                _renderers.Add(rendererViewModel, rendererFile);
            }

            return _renderers[rendererViewModel];
        }

        private IFileRenderer GetRenderer(RendererNodeViewModel rendererViewModel)
        {
            if (rendererViewModel is null)
            {
                return null;
            }

            if (!_renderers.TryGetValue(rendererViewModel, out IFileRenderer renderer))
            {
                return null;
            }

            return renderer;
        }

        public static LibNoiseShaderFile BuildLibNoiseShaderFile(Noise3D noise, NetworkViewModel networkViewModel)
        {
            if (noise is null)
            {
                throw new ArgumentNullException(nameof(noise));
            }

            if (networkViewModel is null)
            {
                throw new ArgumentNullException(nameof(networkViewModel));
            }

            var context = new EditorWriteContext();

            var file = new LibNoiseShaderFile
            {
                HasPositions = true,
                NoiseSeed = noise.Seed
            };

            foreach (NodeViewModel node in networkViewModel.Nodes.Items)
            {
                if (node is not ModuleNodeViewModel viewModel)
                {
                    continue;
                }

                BuildFileModule(file, context, viewModel);
            }

            foreach (NodeViewModel node in networkViewModel.Nodes.Items)
            {
                if (node is not BuilderNodeViewModel viewModel)
                {
                    continue;
                }

                BuildFileBuilder(file, context, viewModel);
            }

            foreach (NodeViewModel node in networkViewModel.Nodes.Items)
            {
                if (node is not RendererNodeViewModel viewModel)
                {
                    continue;
                }

                BuildFileRenderer(file, context, viewModel);
            }

            return file;
        }

        public static LibNoiseShaderFile BuildLibNoiseShaderFile(Noise3D noise, ModuleNodeViewModel viewModel)
        {
            if (noise is null)
            {
                throw new ArgumentNullException(nameof(noise));
            }

            if (viewModel is null)
            {
                throw new ArgumentNullException(nameof(viewModel));
            }

            var context = new EditorWriteContext();

            var file = new LibNoiseShaderFile
            {
                HasPositions = true,
                NoiseSeed = noise.Seed
            };

            BuildFileModule(file, context, viewModel);

            return file;
        }

        public static LibNoiseShaderFile BuildLibNoiseShaderFile(Noise3D noise, BuilderNodeViewModel viewModel)
        {
            if (noise is null)
            {
                throw new ArgumentNullException(nameof(noise));
            }

            if (viewModel is null)
            {
                throw new ArgumentNullException(nameof(viewModel));
            }

            var context = new EditorWriteContext();

            var file = new LibNoiseShaderFile
            {
                HasPositions = true,
                NoiseSeed = noise.Seed
            };

            BuildFileBuilder(file, context, viewModel);

            return file;
        }

        public static LibNoiseShaderFile BuildLibNoiseShaderFile(Noise3D noise, RendererNodeViewModel viewModel)
        {
            if (noise is null)
            {
                throw new ArgumentNullException(nameof(noise));
            }

            if (viewModel is null)
            {
                throw new ArgumentNullException(nameof(viewModel));
            }

            var context = new EditorWriteContext();

            var file = new LibNoiseShaderFile
            {
                HasPositions = true,
                NoiseSeed = noise.Seed
            };

            BuildFileRenderer(file, context, viewModel);

            return file;
        }

        private static IFileModule BuildFileModule(LibNoiseShaderFile file, EditorWriteContext context, ModuleNodeViewModel viewModel)
        {
            if (viewModel is null)
            {
                return null;
            }

            IFileModule module = context.GetModule(viewModel);

            if (module is not null)
            {
                return module;
            }

            module = viewModel switch
            {
                AbsModuleViewModel vm => BuildAbsFileModule(file, context, vm),
                AddModuleViewModel vm => BuildAddFileModule(file, context, vm),
                BillowModuleViewModel vm => BuildBillowFileModule(file, context, vm),
                BlendModuleViewModel vm => BuildBlendFileModule(file, context, vm),
                CacheModuleViewModel vm => BuildCacheFileModule(file, context, vm),
                CheckerboardModuleViewModel vm => BuildCheckerboardFileModule(file, context, vm),
                ClampModuleViewModel vm => BuildClampFileModule(file, context, vm),
                ConstantModuleViewModel vm => BuildConstantFileModule(file, context, vm),
                CurveModuleViewModel vm => BuildCurveFileModule(file, context, vm),
                CylinderModuleViewModel vm => BuildCylinderFileModule(file, context, vm),
                DisplaceModuleViewModel vm => BuildDisplaceFileModule(file, context, vm),
                ExponentModuleViewModel vm => BuildExponentFileModule(file, context, vm),
                InvertModuleViewModel vm => BuildInvertFileModule(file, context, vm),
                LineModuleViewModel vm => BuildLineFileModule(file, context, vm),
                MaxModuleViewModel vm => BuildMaxFileModule(file, context, vm),
                MinModuleViewModel vm => BuildMinFileModule(file, context, vm),
                MultiplyModuleViewModel vm => BuildMultiplyFileModule(file, context, vm),
                PerlinModuleViewModel vm => BuildPerlinFileModule(file, context, vm),
                PowerModuleViewModel vm => BuildPowerFileModule(file, context, vm),
                RidgedMultiModuleViewModel vm => BuildRidgedMultiFileModule(file, context, vm),
                RotatePointModuleViewModel vm => BuildRotatePointFileModule(file, context, vm),
                ScaleBiasModuleViewModel vm => BuildScaleBiasFileModule(file, context, vm),
                ScalePointModuleViewModel vm => BuildScalePointFileModule(file, context, vm),
                SelectorModuleViewModel vm => BuildSelectorFileModule(file, context, vm),
                SphereModuleViewModel vm => BuildSphereFileModule(file, context, vm),
                TerraceModuleViewModel vm => BuildTerraceFileModule(file, context, vm),
                TranslatePointModuleViewModel vm => BuildTranslatePointFileModule(file, context, vm),
                TurbulenceModuleViewModel vm => BuildTurbulenceFileModule(file, context, vm),
                VoronoiModuleViewModel vm => BuildVoronoiFileModule(file, context, vm),
                _ => throw new NotSupportedException($"Not supported module type found: {viewModel.GetType().Name}"),
            };

            module.PositionX = viewModel.Position.X;
            module.PositionY = viewModel.Position.Y;

            context.AddModule(viewModel, module);

            file.Modules.Add(module);
            return module;
        }

        private static IFileModule BuildFileModule(LibNoiseShaderFile file, EditorWriteContext context, ValueNodeInputViewModel<IModule> input)
        {
            return BuildFileModule(file, context, (ModuleNodeViewModel)input.Connections.Items.FirstOrDefault()?.Output.Parent);
        }

        private static IFileModule BuildAbsFileModule(LibNoiseShaderFile file, EditorWriteContext context, AbsModuleViewModel vm)
        {
            var module = new AbsFileModule
            {
                Name = vm.Setting.Name,
                Input1 = BuildFileModule(file, context, vm.Input1),
            };

            return module;
        }

        private static IFileModule BuildAddFileModule(LibNoiseShaderFile file, EditorWriteContext context, AddModuleViewModel vm)
        {
            var module = new AddFileModule
            {
                Name = vm.Setting.Name,
                Input1 = BuildFileModule(file, context, vm.Input1),
                Input2 = BuildFileModule(file, context, vm.Input2),
            };

            return module;
        }

        private static IFileModule BuildBillowFileModule(LibNoiseShaderFile file, EditorWriteContext context, BillowModuleViewModel vm)
        {
            var module = new BillowFileModule
            {
                Name = vm.Setting.Name,
                Frequency = vm.Setting.Frequency,
                Lacunarity = vm.Setting.Lacunarity,
                OctaveCount = vm.Setting.OctaveCount,
                Persistence = vm.Setting.Persistence,
                SeedOffset = vm.Setting.SeedOffset,
            };

            return module;
        }

        private static IFileModule BuildBlendFileModule(LibNoiseShaderFile file, EditorWriteContext context, BlendModuleViewModel vm)
        {
            var module = new BlendFileModule
            {
                Name = vm.Setting.Name,
                Input1 = BuildFileModule(file, context, vm.Input1),
                Input2 = BuildFileModule(file, context, vm.Input2),
                Control = BuildFileModule(file, context, vm.Control),
            };

            return module;
        }

        private static IFileModule BuildCacheFileModule(LibNoiseShaderFile file, EditorWriteContext context, CacheModuleViewModel vm)
        {
            var module = new CacheFileModule
            {
                Name = vm.Setting.Name,
                Input1 = BuildFileModule(file, context, vm.Input1),
            };

            return module;
        }

        private static IFileModule BuildCheckerboardFileModule(LibNoiseShaderFile file, EditorWriteContext context, CheckerboardModuleViewModel vm)
        {
            var module = new CheckerboardFileModule
            {
                Name = vm.Setting.Name,
            };

            return module;
        }

        private static IFileModule BuildClampFileModule(LibNoiseShaderFile file, EditorWriteContext context, ClampModuleViewModel vm)
        {
            var module = new ClampFileModule
            {
                Name = vm.Setting.Name,
                LowerBound = vm.Setting.LowerBound,
                UpperBound = vm.Setting.UpperBound,
                Input1 = BuildFileModule(file, context, vm.Input1),
            };

            return module;
        }

        private static IFileModule BuildConstantFileModule(LibNoiseShaderFile file, EditorWriteContext context, ConstantModuleViewModel vm)
        {
            var module = new ConstantFileModule
            {
                Name = vm.Setting.Name,
                ConstantValue = vm.Setting.ConstantValue,
            };

            return module;
        }

        private static IFileModule BuildCurveFileModule(LibNoiseShaderFile file, EditorWriteContext context, CurveModuleViewModel vm)
        {
            var module = new CurveFileModule
            {
                Name = vm.Setting.Name,
                Input1 = BuildFileModule(file, context, vm.Input1),
            };

            foreach (KeyValuePair<float, float> point in vm.Setting.ControlPoints)
            {
                module.ControlPoints.Add(point);
            }

            return module;
        }

        private static IFileModule BuildCylinderFileModule(LibNoiseShaderFile file, EditorWriteContext context, CylinderModuleViewModel vm)
        {
            var module = new CylinderFileModule
            {
                Name = vm.Setting.Name,
                Frequency = vm.Setting.Frequency,
            };

            return module;
        }

        private static IFileModule BuildDisplaceFileModule(LibNoiseShaderFile file, EditorWriteContext context, DisplaceModuleViewModel vm)
        {
            var module = new DisplaceFileModule
            {
                Name = vm.Setting.Name,
                Input1 = BuildFileModule(file, context, vm.Input1),
                DisplaceX = BuildFileModule(file, context, vm.DisplaceX),
                DisplaceY = BuildFileModule(file, context, vm.DisplaceY),
                DisplaceZ = BuildFileModule(file, context, vm.DisplaceZ),
            };

            return module;
        }

        private static IFileModule BuildExponentFileModule(LibNoiseShaderFile file, EditorWriteContext context, ExponentModuleViewModel vm)
        {
            var module = new ExponentFileModule
            {
                Name = vm.Setting.Name,
                ExponentValue = vm.Setting.ExponentValue,
                Input1 = BuildFileModule(file, context, vm.Input1),
            };

            return module;
        }

        private static IFileModule BuildInvertFileModule(LibNoiseShaderFile file, EditorWriteContext context, InvertModuleViewModel vm)
        {
            var module = new InvertFileModule
            {
                Name = vm.Setting.Name,
                Input1 = BuildFileModule(file, context, vm.Input1),
            };

            return module;
        }

        private static IFileModule BuildLineFileModule(LibNoiseShaderFile file, EditorWriteContext context, LineModuleViewModel vm)
        {
            var module = new LineFileModule
            {
                Name = vm.Setting.Name,
                Attenuate = vm.Setting.Attenuate,
                StartPointX = vm.Setting.StartPointX,
                StartPointY = vm.Setting.StartPointY,
                StartPointZ = vm.Setting.StartPointZ,
                EndPointX = vm.Setting.EndPointX,
                EndPointY = vm.Setting.EndPointY,
                EndPointZ = vm.Setting.EndPointZ,
                Input1 = BuildFileModule(file, context, vm.Input1),
            };

            return module;
        }

        private static IFileModule BuildMaxFileModule(LibNoiseShaderFile file, EditorWriteContext context, MaxModuleViewModel vm)
        {
            var module = new MaxFileModule
            {
                Name = vm.Setting.Name,
                Input1 = BuildFileModule(file, context, vm.Input1),
                Input2 = BuildFileModule(file, context, vm.Input2),
            };

            return module;
        }

        private static IFileModule BuildMinFileModule(LibNoiseShaderFile file, EditorWriteContext context, MinModuleViewModel vm)
        {
            var module = new MinFileModule
            {
                Name = vm.Setting.Name,
                Input1 = BuildFileModule(file, context, vm.Input1),
                Input2 = BuildFileModule(file, context, vm.Input2),
            };

            return module;
        }

        private static IFileModule BuildMultiplyFileModule(LibNoiseShaderFile file, EditorWriteContext context, MultiplyModuleViewModel vm)
        {
            var module = new MultiplyFileModule
            {
                Name = vm.Setting.Name,
                Input1 = BuildFileModule(file, context, vm.Input1),
                Input2 = BuildFileModule(file, context, vm.Input2),
            };

            return module;
        }

        private static IFileModule BuildPerlinFileModule(LibNoiseShaderFile file, EditorWriteContext context, PerlinModuleViewModel vm)
        {
            var module = new PerlinFileModule
            {
                Name = vm.Setting.Name,
                Frequency = vm.Setting.Frequency,
                Lacunarity = vm.Setting.Lacunarity,
                OctaveCount = vm.Setting.OctaveCount,
                Persistence = vm.Setting.Persistence,
                SeedOffset = vm.Setting.SeedOffset,
            };

            return module;
        }

        private static IFileModule BuildPowerFileModule(LibNoiseShaderFile file, EditorWriteContext context, PowerModuleViewModel vm)
        {
            var module = new PowerFileModule
            {
                Name = vm.Setting.Name,
                Input1 = BuildFileModule(file, context, vm.Input1),
                Input2 = BuildFileModule(file, context, vm.Input2),
            };

            return module;
        }

        private static IFileModule BuildRidgedMultiFileModule(LibNoiseShaderFile file, EditorWriteContext context, RidgedMultiModuleViewModel vm)
        {
            var module = new RidgedMultiFileModule
            {
                Name = vm.Setting.Name,
                Frequency = vm.Setting.Frequency,
                Lacunarity = vm.Setting.Lacunarity,
                Offset = vm.Setting.Offset,
                Gain = vm.Setting.Gain,
                Exponent = vm.Setting.Exponent,
                OctaveCount = vm.Setting.OctaveCount,
                SeedOffset = vm.Setting.SeedOffset,
            };

            return module;
        }

        private static IFileModule BuildRotatePointFileModule(LibNoiseShaderFile file, EditorWriteContext context, RotatePointModuleViewModel vm)
        {
            var module = new RotatePointFileModule
            {
                Name = vm.Setting.Name,
                AngleX = vm.Setting.AngleX,
                AngleY = vm.Setting.AngleY,
                AngleZ = vm.Setting.AngleZ,
                Input1 = BuildFileModule(file, context, vm.Input1),
            };

            return module;
        }

        private static IFileModule BuildScaleBiasFileModule(LibNoiseShaderFile file, EditorWriteContext context, ScaleBiasModuleViewModel vm)
        {
            var module = new ScaleBiasFileModule
            {
                Name = vm.Setting.Name,
                Bias = vm.Setting.Bias,
                Scale = vm.Setting.Scale,
                Input1 = BuildFileModule(file, context, vm.Input1),
            };

            return module;
        }

        private static IFileModule BuildScalePointFileModule(LibNoiseShaderFile file, EditorWriteContext context, ScalePointModuleViewModel vm)
        {
            var module = new ScalePointFileModule
            {
                Name = vm.Setting.Name,
                ScaleX = vm.Setting.ScaleX,
                ScaleY = vm.Setting.ScaleY,
                ScaleZ = vm.Setting.ScaleZ,
                Input1 = BuildFileModule(file, context, vm.Input1),
            };

            return module;
        }

        private static IFileModule BuildSelectorFileModule(LibNoiseShaderFile file, EditorWriteContext context, SelectorModuleViewModel vm)
        {
            var module = new SelectorFileModule
            {
                Name = vm.Setting.Name,
                EdgeFalloff = vm.Setting.EdgeFalloff,
                LowerBound = vm.Setting.LowerBound,
                UpperBound = vm.Setting.UpperBound,
                Input1 = BuildFileModule(file, context, vm.Input1),
                Input2 = BuildFileModule(file, context, vm.Input2),
                Control = BuildFileModule(file, context, vm.Control),
            };

            return module;
        }

        private static IFileModule BuildSphereFileModule(LibNoiseShaderFile file, EditorWriteContext context, SphereModuleViewModel vm)
        {
            var module = new SphereFileModule
            {
                Name = vm.Setting.Name,
                Frequency = vm.Setting.Frequency,
            };

            return module;
        }

        private static IFileModule BuildTerraceFileModule(LibNoiseShaderFile file, EditorWriteContext context, TerraceModuleViewModel vm)
        {
            var module = new TerraceFileModule
            {
                Name = vm.Setting.Name,
                IsInverted = vm.Setting.IsInverted,
                Input1 = BuildFileModule(file, context, vm.Input1),
            };

            foreach (float point in vm.Setting.ControlPoints)
            {
                module.ControlPoints.Add(point);
            }

            return module;
        }

        private static IFileModule BuildTranslatePointFileModule(LibNoiseShaderFile file, EditorWriteContext context, TranslatePointModuleViewModel vm)
        {
            var module = new TranslatePointFileModule
            {
                Name = vm.Setting.Name,
                TranslateX = vm.Setting.TranslateX,
                TranslateY = vm.Setting.TranslateY,
                TranslateZ = vm.Setting.TranslateZ,
                Input1 = BuildFileModule(file, context, vm.Input1),
            };

            return module;
        }

        private static IFileModule BuildTurbulenceFileModule(LibNoiseShaderFile file, EditorWriteContext context, TurbulenceModuleViewModel vm)
        {
            var module = new TurbulenceFileModule
            {
                Name = vm.Setting.Name,
                Frequency = vm.Setting.Frequency,
                Power = vm.Setting.Power,
                Roughness = vm.Setting.Roughness,
                SeedOffset = vm.Setting.SeedOffset,
                Input1 = BuildFileModule(file, context, vm.Input1),
            };

            return module;
        }

        private static IFileModule BuildVoronoiFileModule(LibNoiseShaderFile file, EditorWriteContext context, VoronoiModuleViewModel vm)
        {
            var module = new VoronoiFileModule
            {
                Name = vm.Setting.Name,
                IsDistanceApplied = vm.Setting.IsDistanceApplied,
                Displacement = vm.Setting.Displacement,
                Frequency = vm.Setting.Frequency,
                SeedOffset = vm.Setting.SeedOffset,
            };

            return module;
        }

        private static IFileBuilder BuildFileBuilder(LibNoiseShaderFile file, EditorWriteContext context, BuilderNodeViewModel viewModel)
        {
            if (viewModel is null)
            {
                return null;
            }

            IFileBuilder builder = context.GetBuilder(viewModel);

            if (builder is not null)
            {
                return builder;
            }

            builder = viewModel switch
            {
                CylinderBuilderViewModel vm => BuildCylinderFileBuilder(file, context, vm),
                PlaneBuilderViewModel vm => BuildPlaneFileBuilder(file, context, vm),
                SphereBuilderViewModel vm => BuildSphereFileBuilder(file, context, vm),
                _ => throw new NotSupportedException($"Not supported builder type found: {viewModel.GetType().Name}"),
            };

            builder.PositionX = viewModel.Position.X;
            builder.PositionY = viewModel.Position.Y;

            context.AddBuilder(viewModel, builder);

            file.Builders.Add(builder);
            return builder;
        }

        private static IFileBuilder BuildFileBuilder(LibNoiseShaderFile file, EditorWriteContext context, ValueNodeInputViewModel<IBuilder> input)
        {
            return BuildFileBuilder(file, context, (BuilderNodeViewModel)input.Connections.Items.FirstOrDefault()?.Output.Parent);
        }

        private static IFileBuilder BuildCylinderFileBuilder(LibNoiseShaderFile file, EditorWriteContext context, CylinderBuilderViewModel vm)
        {
            var builder = new CylinderFileBuilder
            {
                Name = vm.Setting.Name,
                LowerAngleBound = vm.Setting.LowerAngleBound,
                LowerHeightBound = vm.Setting.LowerHeightBound,
                UpperAngleBound = vm.Setting.UpperAngleBound,
                UpperHeightBound = vm.Setting.UpperHeightBound,
                Source = BuildFileModule(file, context, vm.Source),
            };

            return builder;
        }

        private static IFileBuilder BuildPlaneFileBuilder(LibNoiseShaderFile file, EditorWriteContext context, PlaneBuilderViewModel vm)
        {
            var builder = new PlaneFileBuilder
            {
                Name = vm.Setting.Name,
                IsSeamless = vm.Setting.IsSeamless,
                LowerBoundX = vm.Setting.LowerBoundX,
                UpperBoundX = vm.Setting.UpperBoundX,
                LowerBoundY = vm.Setting.LowerBoundY,
                UpperBoundY = vm.Setting.UpperBoundY,
                Source = BuildFileModule(file, context, vm.Source),
            };

            return builder;
        }

        private static IFileBuilder BuildSphereFileBuilder(LibNoiseShaderFile file, EditorWriteContext context, SphereBuilderViewModel vm)
        {
            var builder = new SphereFileBuilder
            {
                Name = vm.Setting.Name,
                SouthLatBound = vm.Setting.SouthLatBound,
                NorthLatBound = vm.Setting.NorthLatBound,
                WestLonBound = vm.Setting.WestLonBound,
                EastLonBound = vm.Setting.EastLonBound,
                Source = BuildFileModule(file, context, vm.Source),
            };

            return builder;
        }

        private static IFileRenderer BuildFileRenderer(LibNoiseShaderFile file, EditorWriteContext context, RendererNodeViewModel viewModel)
        {
            if (viewModel is null)
            {
                return null;
            }

            IFileRenderer renderer = context.GetRenderer(viewModel);

            if (renderer is not null)
            {
                return renderer;
            }

            renderer = viewModel switch
            {
                BlendRendererViewModel vm => BuildBlendFileRenderer(file, context, vm),
                ImageRendererViewModel vm => BuildImageFileRenderer(file, context, vm),
                NormalRendererViewModel vm => BuildNormalFileRenderer(file, context, vm),
                _ => throw new NotSupportedException($"Not supported renderer type found: {viewModel.GetType().Name}"),
            };

            renderer.PositionX = viewModel.Position.X;
            renderer.PositionY = viewModel.Position.Y;

            context.AddRenderer(viewModel, renderer);

            file.Renderers.Add(renderer);
            return renderer;
        }

        private static IFileRenderer BuildFileRenderer(LibNoiseShaderFile file, EditorWriteContext context, ValueNodeInputViewModel<IRenderer> input)
        {
            return BuildFileRenderer(file, context, (RendererNodeViewModel)input.Connections.Items.FirstOrDefault()?.Output.Parent);
        }

        private static IFileRenderer BuildBlendFileRenderer(LibNoiseShaderFile file, EditorWriteContext context, BlendRendererViewModel vm)
        {
            var renderer = new BlendFileRenderer
            {
                Name = vm.Setting.Name,
                Input1 = BuildFileRenderer(file, context, vm.Input1),
                Input2 = BuildFileRenderer(file, context, vm.Input2),
            };

            return renderer;
        }

        private static IFileRenderer BuildImageFileRenderer(LibNoiseShaderFile file, EditorWriteContext context, ImageRendererViewModel vm)
        {
            var renderer = new ImageFileRenderer
            {
                Name = vm.Setting.Name,
                IsWrapEnabled = vm.Setting.IsWrapEnabled,
                IsLightEnabled = vm.Setting.IsLightEnabled,
                LightAzimuth = vm.Setting.LightAzimuth,
                LightBrightness = vm.Setting.LightBrightness,
                LightColor = Color.FromArgb(vm.Setting.LightColor.A, vm.Setting.LightColor.R, vm.Setting.LightColor.G, vm.Setting.LightColor.B),
                LightContrast = vm.Setting.LightContrast,
                LightElevation = vm.Setting.LightElevation,
                LightIntensity = vm.Setting.LightIntensity,
                Source = BuildFileBuilder(file, context, vm.Source),
            };

            foreach (KeyValuePair<float, System.Windows.Media.Color> point in vm.Setting.GradientPoints)
            {
                float key = point.Key;
                Color color = Color.FromArgb(point.Value.A, point.Value.R, point.Value.G, point.Value.B);

                renderer.GradientPoints.Add(key, color);
            }

            return renderer;
        }

        private static IFileRenderer BuildNormalFileRenderer(LibNoiseShaderFile file, EditorWriteContext context, NormalRendererViewModel vm)
        {
            var renderer = new NormalFileRenderer
            {
                Name = vm.Setting.Name,
                Source = BuildFileBuilder(file, context, vm.Source),
            };

            return renderer;
        }
    }
}

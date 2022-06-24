using NodeNetwork.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibNoiseShaderEditor.Models
{
    public sealed class NodesLayoutContext
    {
        private readonly Dictionary<int, List<NodeViewModel>> _nodes = new();

        private void AddNode(NodeViewModel nodeViewModel, int offset)
        {
            if (!_nodes.ContainsKey(offset))
            {
                _nodes.Add(offset, new List<NodeViewModel>());
            }

            List<NodeViewModel> currentNode = _nodes[offset];

            if (!currentNode.Contains(nodeViewModel))
            {
                if (offset > 1)
                {
                    int minPositionY = _nodes[offset - 1].Count - 1;

                    for (int y = currentNode.Count; y < minPositionY; y++)
                    {
                        currentNode.Add(null);
                    }
                }

                currentNode.Add(nodeViewModel);
            }
        }

        private void SetPositions()
        {
            int nodesWidth = _nodes.Values.Count;
            int nodesHeight = _nodes.Values.Count == 0 ? 0 : _nodes.Values.Max(t => t.Count);

            double[] widths = new double[nodesWidth];

            for (int nodeX = 0; nodeX < nodesWidth; nodeX++)
            {
                double columnWidth = _nodes.Values
                    .ElementAt(nodeX)
                    .Max(t => t is null ? 0.0 : t.Size.Width);

                widths[nodeX] = columnWidth;
            }

            double[] heights = new double[nodesHeight];

            for (int nodeY = 0; nodeY < nodesHeight; nodeY++)
            {
                double rowHeight = _nodes.Values
                    .Select(t => t.ElementAtOrDefault(nodeY))
                    .Max(t => t is null ? 0.0 : t.Size.Height);

                heights[nodeY] = rowHeight;
            }

            double positionX = 0;

            for (int nodeX = 0; nodeX < nodesWidth; nodeX++)
            {
                double positionY = 0;

                for (int nodeY = 0; nodeY < nodesHeight; nodeY++)
                {
                    NodeViewModel node = _nodes.Values
                        .ElementAt(nodeX)
                        .ElementAtOrDefault(nodeY);

                    if (node is not null)
                    {
                        node.Position = new System.Windows.Point(positionX, positionY);
                    }

                    positionY += heights[nodeY] + GlobalConstants.NetworkNodesLayoutDeltaY;
                }

                positionX += widths[nodeX] + GlobalConstants.NetworkNodesLayoutDeltaX;
            }
        }

        public static void BuildLayout(NetworkViewModel networkViewModel)
        {
            if (networkViewModel is null)
            {
                throw new ArgumentNullException(nameof(networkViewModel));
            }

            var context = new NodesLayoutContext();

            foreach (NodeViewModel node in networkViewModel.Nodes.Items)
            {
                int outputCount = node.Outputs.Items.FirstOrDefault()?.Connections.Count ?? 0;

                if (outputCount != 0)
                {
                    continue;
                }

                BuildNodeLayout(context, node);
            }

            using var _ = networkViewModel.DelayChangeNotifications();
            context.SetPositions();
        }

        private static int BuildNodeLayout(NodesLayoutContext context, NodeViewModel viewModel)
        {
            if (context is null)
            {
                return 0;
            }

            if (viewModel is null)
            {
                return 0;
            }

            int offset = 0;

            foreach (NodeInputViewModel input in viewModel.Inputs.Items)
            {
                offset = Math.Max(offset, BuildNodeLayout(context, input.Connections.Items.FirstOrDefault()?.Output.Parent));
            }

            offset++;

            context.AddNode(viewModel, offset);

            return offset;
        }
    }
}

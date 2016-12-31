using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using Device = SharpDX.Direct3D11.Device;
using MapFlags = SharpDX.Direct3D11.MapFlags;
using dxResource = SharpDX.DXGI.Resource;

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using SharpDX.Mathematics.Interop;

namespace NextCapture.Core
{
    class DxCapture : BaseCapture
    {
        Factory1 factory = new Factory1();
        
        private bool GetAdapterFromPopint(Point point, out Adapter adapter, out Output output)
        {
            string device = Screen.FromPoint(point).DeviceName;

            output = null;
            adapter = null;
            
            foreach (var ad in factory.Adapters)
            {
                for (int i = 0; i < ad.GetOutputCount(); i++)
                {
                    var op = ad.Outputs[i];

                    var bound = RawToRectangle(op.Description.DesktopBounds);
                    
                    if (bound.Contains(point))
                    {
                        output = op;
                        adapter = ad;

                        return true;
                    }
                }
            }

            return false;
        }

        private Rectangle RawToRectangle(RawRectangle rawRect)
        {
            return new Rectangle()
            {
                X = rawRect.Left,
                Y = rawRect.Top,
                Width = rawRect.Right - rawRect.Left,
                Height = rawRect.Bottom - rawRect.Top
            };
        }

        protected override Bitmap OnCapture(Rectangle area)
        {
            Bitmap result = null;

            Adapter adapter;
            Output output;

            if (!GetAdapterFromPopint(area.Location, out adapter, out output))
                return null;

            var device = new Device(adapter);
            var output1 = output.QueryInterface<Output1>();

            var scrBound = RawToRectangle(output.Description.DesktopBounds);

            area.Location -= new Size(scrBound.X, scrBound.Y);

            var textureDesc = new Texture2DDescription()
            {
                CpuAccessFlags = CpuAccessFlags.Read,
                BindFlags = BindFlags.None,
                Format = Format.B8G8R8A8_UNorm,
                Width = scrBound.Width,
                Height = scrBound.Height,
                OptionFlags = ResourceOptionFlags.None,
                MipLevels = 1,
                ArraySize = 1,
                SampleDescription = 
                {
                    Count = 1,
                    Quality = 0
                },
                Usage = ResourceUsage.Staging
            };
            
            var scrTexture = new Texture2D(device, textureDesc);
            var dupOutput = output1.DuplicateOutput(device);

            bool done = false;
            
            for (int i = 0; !done; i++)
            {
                try
                {
                    dxResource scrResource;
                    OutputDuplicateFrameInformation dupFrameInfo;

                    dupOutput.AcquireNextFrame(10000, out dupFrameInfo, out scrResource);
                    
                    if (i > 0)
                    {
                        using (var scrTexture2D = scrResource.QueryInterface<Texture2D>())
                            device.ImmediateContext.CopyResource(scrTexture2D, scrTexture);

                        var mapSrc = device.ImmediateContext.MapSubresource(scrTexture, 0, MapMode.Read, MapFlags.None);
                        
                        var bmp = new Bitmap(area.Width, area.Height, PixelFormat.Format32bppRgb);
                        var rect = new Rectangle(0, 0, area.Width, area.Height);

                        var mapDest = bmp.LockBits(rect, ImageLockMode.WriteOnly, bmp.PixelFormat);
                        var srcPtr = mapSrc.DataPointer;
                        var destPtr = mapDest.Scan0;
                        
                        // 1 픽셀당 바이트 크기
                        int rowPitchBit = mapSrc.RowPitch / scrBound.Width;
                        int stridePerBit = mapDest.Stride / area.Width;
                        
                        for (int y = 0; y < area.Top; y++)
                            srcPtr = IntPtr.Add(srcPtr, mapSrc.RowPitch);

                        for (int y = area.Top; y < Math.Min(area.Bottom, scrBound.Height); y++)
                        {
                            int movePitch = rowPitchBit * area.Left;

                            // 소스 포인터 앞부분 이동 (Left)
                            srcPtr = IntPtr.Add(srcPtr, movePitch);

                            // 복사
                            Utilities.CopyMemory(destPtr, srcPtr, Math.Min(area.Width * 4, mapSrc.RowPitch - movePitch));

                            // 소스 포인터 뒷부분 매움
                            srcPtr = IntPtr.Add(srcPtr, mapSrc.RowPitch - movePitch);
                            destPtr = IntPtr.Add(destPtr, mapDest.Stride);
                        }

                        bmp.UnlockBits(mapDest);
                        device.ImmediateContext.UnmapSubresource(scrTexture, 0);

                        result = bmp;

                        done = true;
                    }

                    scrResource.Dispose();
                    dupOutput.ReleaseFrame();
                }
                catch (SharpDXException e)
                {
                    if (e.ResultCode.Code != SharpDX.DXGI.ResultCode.WaitTimeout.Result.Code)
                    {
                        done = true;
                        new MessageWindow("Error", e.ToString()).Show();
                    }
                }
            }

            dupOutput.Dispose();
            scrTexture.Dispose();
            output1.Dispose();
            output.Dispose();
            device.Dispose();
            
            return result;
        }

        protected override Bitmap OnCapture(IntPtr hwnd, Rectangle area)
        {
            return null;
        }
    }
}

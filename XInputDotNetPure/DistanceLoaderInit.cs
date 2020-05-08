namespace XInputDotNetPure
{
    class DistanceLoaderInit
    {
        public void StartCore()
        {
            DistanceLoader.Util.Logger.Instance.Log("Starting Distance Loader Core");
            var distanceCore = new DistanceLoader.Loader.Core.Loader();
            distanceCore.Init();
        }
    }
}

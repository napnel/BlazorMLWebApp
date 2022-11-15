let session;

async function runOnnxRunTime(modelPath, image)
{
    console.log("runOnnxRunTime called");
    try {

        if (session == null)
        {
            session = await ort.InferenceSession.create(modelPath);
        }
        const input = Float32Array.from(image);
        const tensor = new ort.Tensor('float32', input, [1, 3, 224, 224]);
        const results = await session.run({ 'data_0': tensor});
        return Array.from(results["softmaxout_1"].data);
    } catch (e) {
        console.log("failed to run onnx runtime: ${e}");
    }
}
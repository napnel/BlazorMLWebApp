# How to infer with onnx runtime via Javascript

The javascript folder location indicates wwwroot/js/runOnnxRunTime.js.

```Javascript
<script src="https://cdn.jsdelivr.net/npm/onnxruntime-web/dist/ort.min.js"></script>
<script src="js/runOnnxRunTime.js"></script>
```

The following Javascript function is called in the .razor file.

```c#
@inject IJSRuntime JS
async Task SomeFunction()
{
    // If Javascript function have float[] return
    await JS.InvokeAsync<float[]>("runOnnxRunTime", args)

    // If no return
    await JS.InvokeVoidAsync("runOnnxRunTime", args)
}
```

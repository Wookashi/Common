[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![MIT License][license-shield]][license-url]
[![LinkedIn][linkedin-shield]][linkedin-url]



<!-- PROJECT LOGO -->
<br />
<p align="center">
  <a href="https://github.com/Wookashi/Result">
    <img src="Result/icon.png" alt="Logo" width="80" height="80">
  </a>

  <h3 align="center">Wookashi Result</h3>

  <p align="center">
    A simple helper for returning execution status from methods.
    <br/>
    <br/>
    <a href="https://github.com/Wookashi/Result">View Demo</a>
    ·
    <a href="https://github.com/Wookashi/Result/issues">Report Bug</a>
    ·
    <a href="https://github.com/Wookashi/Result/issues">Request Feature</a>
  </p>
</p>



<!-- TABLE OF CONTENTS -->
<details open="open">
  <summary><h2 style="display: inline-block">Table of Contents</h2></summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#installation">Installation</a></li>
		<li><a href="#basic-usage">Basic Usage</a></li>
      </ul>
    </li>
    	<li><a href="#roadmap">Roadmap</a></li>
		<li><a href="#contributing">Contributing</a></li>
		<li><a href="#license">License</a></li>
  </ol>
</details>



<!-- ABOUT THE PROJECT -->
## About The Project

Library solve problem with returning unnown state from methods. Instead of returning void we can return Result. Result can include generic data in it (DataResult\<T\>). Moreover we can return collection of Results as ResultsPack.


### Installation

Wookashi.Common.Result is available on <a href="https://www.nuget.org/packages/Wookashi.Common.Result/">NuGet</a>.


### Basic usage

The following code demonstrates simple example code without Wookashi Result Library.

```cs
        void SetInt(int[] array, int index, int value)
        {
            try
            {
                array[index] = value;
            }
            catch (IndexOutOfRangeException e)
            {
                throw new ArgumentOutOfRangeException(
                    "Parameter index is out of range.", e);
            }
        }

        int GetInt(int[] array, int index)
        {
            try
            {
                return array[index];
            }
            catch (IndexOutOfRangeException e)
            {
                throw new ArgumentOutOfRangeException(
                    "Parameter index is out of range.", e);
            }
        }
```

Same code with Wookashi Result Library below:
```cs
        Result SetInt(int[] array, int index, int value)
        {
            try
            {
                array[index] = value;
                return Result.Success;
            }
            catch (IndexOutOfRangeException e)
            {
                return Result.Error(
                    "Parameter {Index} is out of range.", index).SetException(e);
            }
        }

        DataResult<int> GetInt(int[] array, int index)
        {
            try
            {
                return DataResult<int>.SuccessWithData(array[index]);
            }
            catch (IndexOutOfRangeException e)
            {
                return DataResult<int>.Error(
                    "Parameter {Index} is out of range.", index).SetException(e);
            }
        }
```

The following code demonstrates usage of ResultPack.
```cs
        ResultsPack SetInts(int[] array, List<(int index, int value)> values)
        {
            var resPack = new ResultsPack();
            foreach (var kvPair in values)
            {
                try
                {
                    array[kvPair.index] = kvPair.value;
                    resPack.Add(Result.SuccessWithMessage("Value {Value} sets sucessfully at index {Index}", kvPair.value, kvPair.index));
                }
                catch (IndexOutOfRangeException e)
                {
                    resPack.Add(Result.Error("Parameter {Index} is out of range.", kvPair.index).SetException(e));
                }
            }
            return resPack;
        }
```


<!-- ROADMAP -->
## Roadmap

See the [open issues](https://github.com/Wookashi/Result/issues) for a list of proposed features (and known issues).



<!-- CONTRIBUTING -->
## Contributing

Contributions are what make the open source community such an amazing place to be learn, inspire, and create. Any contributions you make are **greatly appreciated**.

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request



<!-- LICENSE -->
## License

Distributed under the MIT License. See `LICENSE` for more information.



<!-- CONTACT -->
## Contact

Wookashi - [@https://twitter.com/lukaszhr](https://twitter.com/https://twitter.com/lukaszhr) - lukasz.hr@outlook.com

Project Link: [https://github.com/Wookashi/Result](https://github.com/Wookashi/Result)


<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[contributors-shield]: https://img.shields.io/github/contributors/Wookashi/Result.svg?style=for-the-badge
[contributors-url]: https://github.com/Wookashi/Result/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/Wookashi/Result.svg?style=for-the-badge
[forks-url]: https://github.com/Wookashi/Result/network/members
[stars-shield]: https://img.shields.io/github/stars/Wookashi/Result.svg?style=for-the-badge
[stars-url]: https://github.com/Wookashi/Result/stargazers
[issues-shield]: https://img.shields.io/github/issues/Wookashi/Result.svg?style=for-the-badge
[issues-url]: https://github.com/Wookashi/Result/issues
[license-shield]: https://img.shields.io/github/license/Wookashi/Result.svg?style=for-the-badge
[license-url]: https://github.com/Wookashi/Result/blob/master/LICENSE
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://www.linkedin.com/in/lukaszhryciuk/
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Deployment.Core.Deployment.VersionDeployment;

public sealed class Version700Deployment : IVersionDeployment
{
    public string Version => "7.0.0";

    public Task ApplyAsync(
        string versionPath,
        string websitePath,
        bool isPreviewRun)
    {
        FlattenContainerDataItemReferences(
            websitePath,
            isPreviewRun);

        AddTabInfoNamespaceImports(
            websitePath,
            isPreviewRun);

        AddMegaMenuInfoNamespaceImports(
            websitePath,
            isPreviewRun);

        MergeAssemblyBindings(
            versionPath,
            websitePath,
            isPreviewRun);

        return Task.CompletedTask;
    }

    private static void FlattenContainerDataItemReferences(
        string websitePath,
        bool isPreviewRun)
    {
        string desktopComponentsPath = Path.Combine(
            websitePath,
            "DesktopComponents");

        if (!Directory.Exists(desktopComponentsPath))
            throw new DirectoryNotFoundException($"Target 'DesktopComponents' was not found: {desktopComponentsPath}");

        foreach (string file in Directory.EnumerateFiles(
            desktopComponentsPath,
            "*",
            SearchOption.AllDirectories))
        {
            if (!file.EndsWith("*.aspx", StringComparison.OrdinalIgnoreCase) &&
                !file.EndsWith("*.ascx", StringComparison.OrdinalIgnoreCase))
                continue;

            if (isPreviewRun)
                continue;

            string content = File.ReadAllText(file);
            string updatedContent = content
                .Replace(
                "Container.DataItem).Info?",
                "Container.DataItem)")
                .Replace(
                "Container.DataItem).AuditedInfo?",
                "Container.DataItem)");

            if (content != updatedContent)
            {
                File.WriteAllText(file, updatedContent);
            }
        }
    }

    private static void AddTabInfoNamespaceImports(
        string websitePath,
        bool isPreviewRun)
    {
        string desktopModulesPath = Path.Combine(
            websitePath,
            "DesktopModules");

        string import =
            Environment.NewLine +
            "<%@ Import Namespace=\"MyPortal.BusinessEntities.Tabs\" %>";

        foreach (string file in Directory.EnumerateFiles(
            desktopModulesPath,
            "*.ascx",
            SearchOption.AllDirectories))
        {
            string content = File.ReadAllText(file);

            if (!content.Contains("TabInfo", StringComparison.Ordinal))
                continue;

            if (content.Contains(
                "MyPortal.BusinessEntities.Tabs",
                StringComparison.Ordinal))
                continue;

            int index = GetControlDirectiveEnd(file, content);

            string updatedContent =
                content.Insert(index, import);

            if (!isPreviewRun)
                File.WriteAllText(file, updatedContent);
        }
    }

    private static void AddMegaMenuInfoNamespaceImports(
        string websitePath,
        bool isPreviewRun)
    {
        string desktopModulesPath = Path.Combine(
            websitePath,
            "DesktopModules");

        string import =
            Environment.NewLine +
            "<%@ Import Namespace=\"MyPortal.BusinessEntities.MegaMenu.Display\" %>";

        IEnumerable<string> files =
            Directory.EnumerateFiles(
                websitePath,
                "*.ascx",
                SearchOption.TopDirectoryOnly)
            .Concat(
                Directory.EnumerateFiles(
                    desktopModulesPath,
                    "*.ascx",
                    SearchOption.AllDirectories));

        foreach (string file in files)
        {
            string content = File.ReadAllText(file);

            if (!content.Contains(
                "MegaMenuInfo",
                StringComparison.Ordinal))
                continue;

            if (content.Contains(
                "MyPortal.BusinessEntities.MegaMenu.Display",
                StringComparison.Ordinal))
                continue;

            int index = GetControlDirectiveEnd(file, content);

            string updatedContent =
                content.Insert(index, import);

            if (!isPreviewRun)
                File.WriteAllText(file, updatedContent);
        }
    }
    private static int GetControlDirectiveEnd(string file, string content)
    {
        string directive = "<%@ Control";
        int directiveStart = content.IndexOf(
            directive,
            StringComparison.OrdinalIgnoreCase);

        if (directiveStart < 0)
            throw new InvalidOperationException(
                $"Could not find directive in file: {file}");

        int directiveEnd = content.IndexOf(
            "%>",
            directiveStart,
            StringComparison.Ordinal);

        if (directiveEnd < 0)
            throw new InvalidOperationException(
                $"Could not find end of directive in file: {file}");

        return directiveEnd += 2;
    }


    private static void MergeAssemblyBindings(
        string versionPath,
        string websitePath,
        bool isPreviewRun)
    {
        string sourceWebConfig = Path.Combine(
            versionPath,
            "build",
            "Web.config");

        string targetWebConfig = Path.Combine(
            websitePath,
            "Web.config");

        if (!File.Exists(sourceWebConfig))
            throw new FileNotFoundException(
                $"Source Web.conif was not found: {sourceWebConfig}");

        if (!File.Exists(targetWebConfig))
            throw new FileNotFoundException(
                $"Target Web.config was not found: {targetWebConfig}");

        const string assemblyBindingNamespace =
            "urn:schemas-microsoft-com:asm.v1";

        XmlDocument source = new()
        {
            PreserveWhitespace = true
        };
        source.Load(sourceWebConfig);

        XmlDocument target = new()
        {
            PreserveWhitespace = true
        };
        target.Load(targetWebConfig);

        XmlNamespaceManager sourceNamespaces =
            CreateNamespaceManager(source);
        XmlNamespaceManager targetNamespaces =
            CreateNamespaceManager(target);

        XmlNode? sourceAssemblyBinding =
            source.SelectSingleNode(
                "/configuration/runtime/asm:assemblyBinding",
                sourceNamespaces);

        if (sourceAssemblyBinding is null)
            return;

        XmlNode? targetRuntime =
            target.SelectSingleNode(
                "/configuration/runtime");

        if (targetRuntime is null)
        {
            targetRuntime = target.CreateElement("runtime");
            target.DocumentElement!.AppendChild(targetRuntime);
        }

        XmlNodeList targetAssemblyBindings =
            targetRuntime.SelectNodes(
                "asm:assemblyBinding",
                targetNamespaces)!;

        XmlNode targetAssemblyBinding;

        if (targetAssemblyBindings.Count == 0)
        {
            targetAssemblyBinding = target.CreateElement(
                "assemblyBinding",
                assemblyBindingNamespace);

            targetRuntime.AppendChild(targetAssemblyBinding);
        }
        else
        {
            targetAssemblyBinding = targetAssemblyBindings[0]!;
        }

        // Clean up additional <assemblyBinding> tags 
        for (int i = targetAssemblyBindings.Count - 1; i > 0; i--)
        {
            XmlNode additionalAssemblyBinding =
                targetAssemblyBindings[i]!;

            while (additionalAssemblyBinding.HasChildNodes)
            {
                XmlNode child =
                    additionalAssemblyBinding.FirstChild!;

                additionalAssemblyBinding.RemoveChild(child);

                if (child.Name == "dependentAssembly")
                {
                    targetAssemblyBinding.AppendChild(child);
                }
            }

            targetRuntime.RemoveChild(additionalAssemblyBinding);
        }

        foreach (XmlNode sourceBinding in sourceAssemblyBinding.ChildNodes)
        {
            if (sourceBinding.Name != "dependentAssembly")
                continue;

            XmlNodeList existingBindings =
                targetAssemblyBinding.SelectNodes(
                    "asm:dependentAssembly",
                    targetNamespaces)!;

            foreach (XmlNode existingBinding in existingBindings)
            {
                if (HasSameAssemblyIdentity(
                    sourceBinding,
                    existingBinding,
                    targetNamespaces))
                    targetAssemblyBinding.RemoveChild(existingBinding);
            }

            XmlNode importedBinding
                = target.ImportNode(sourceBinding, deep: true);

            targetAssemblyBinding.AppendChild(importedBinding);
        }

        if (!isPreviewRun)
            target.Save(targetWebConfig);
    }

    private static bool HasSameAssemblyIdentity(
        XmlNode sourceBinding,
        XmlNode targetBinding,
        XmlNamespaceManager namespaces)
    {
        XmlElement? sourceIdentity =
            sourceBinding.SelectSingleNode(
                "asm:assemblyIdentity",
                namespaces) as XmlElement;

        XmlElement? targetIdentity =
            targetBinding.SelectSingleNode(
                "asm:assemblyIdentity",
                namespaces) as XmlElement;

        if (sourceIdentity is null || targetIdentity is null)
            return false;

        return string.Equals(
            sourceIdentity.GetAttribute("name"),
            targetIdentity.GetAttribute("name"),
            StringComparison.OrdinalIgnoreCase)
            && string.Equals(
                sourceIdentity.GetAttribute("publicKeyToken"),
                targetIdentity.GetAttribute("publicKeyToken"),
                StringComparison.OrdinalIgnoreCase)
            && string.Equals(
                sourceIdentity.GetAttribute("culture"),
                targetIdentity.GetAttribute("culture"),
                StringComparison.OrdinalIgnoreCase);
    }

    private static XmlNamespaceManager CreateNamespaceManager(
        XmlDocument document)
    {
        XmlNamespaceManager namespaces = new(
            document.NameTable);

        namespaces.AddNamespace(
            "asm",
            "urn:schemas-microsoft-com:asm.v1");

        return namespaces;
    }
}
